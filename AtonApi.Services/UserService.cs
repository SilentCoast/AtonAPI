using AtonAPI.Data;
using AtonAPI.Data.Models;
using AtonAPI.Data.Repositories;

namespace AtonAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<(UserDTO userDTO, bool success, string errorMessage)> GetUserDTOByLoginAsync(string login)
        {
            try
            {
                var user = _userRepository.GetUserByLogin(login);
                if (user != null)
                {
                    UserDTO userDTO = new UserDTO
                    {
                        Name = user.Name,
                        Birthday = user.Birthday,
                        Gender = user.Gender,
                        IsActive = (user.RevokedOn == null)
                    };
                    return (userDTO, true, null);
                }
                return (null, false, null);
            }
            catch (Exception ex)
            {
                return (null, false, ex.Message);
            }
        }

        public async Task<(bool success, string errorMessage)> CreateUserAsync(UserCreateModel userCreateModel, string createdByLogin)
        {
            try
            {
                User user = new User
                {
                    Name = userCreateModel.Name,
                    Gender = userCreateModel.Gender,
                    Admin = userCreateModel.IsAdmin,
                    Birthday = userCreateModel.Birthday,
                    Password = userCreateModel.Password,
                    Login = userCreateModel.Login
                };
                _userRepository.CreateUser(user, createdByLogin);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        /// <summary>
        /// check if user allowed to modify. user allowed to modify if they modifying themselfs(and they are Active, e.g. RevokedOn is null) or they are Admin
        /// </summary>
        /// <param name="targetLogin">login of a user to be modified</param>
        /// <param name="modifyingByLogin">user that performs modification</param>
        /// <returns></returns>
        private (bool success, string errorMessage) CheckAccess(string targetLogin, string modifyingByLogin)
        {
            User modifyingBy = _userRepository.GetUserByLogin(modifyingByLogin);
            if (modifyingBy != null)
            {
                if (!modifyingBy.Admin)
                {
                    if (targetLogin == modifyingBy.Login)
                    {
                        if (modifyingBy.RevokedOn != null)
                        {
                            return (false, "Access denied");
                        }
                    }
                    else
                    {
                        return (false, "Access denied");
                    }
                }
            }
            else
            {
                return (false, "user sending request does not exist");
            }
            return (true, null);
        }
        public async Task<(bool success, string errorMessage)> UpdateUserInfoAsync(string login, string modifiedByLogin, string? name = null, int? gender = null, DateTime? birthDay = null)
        {
            try
            {
                var result = CheckAccess(login, modifiedByLogin);
                if (!result.success)
                {
                    return (false, result.errorMessage);
                }

                User user = _userRepository.GetUserByLogin(login);
                if (user == null)
                {
                    return (false, "user not found");
                }
                if (name != null)
                {
                    user.Name = name;
                }
                if (gender != null)
                {
                    user.Gender = (int)gender;
                }
                if (birthDay != null)
                {
                    user.Birthday = (DateTime)birthDay;
                }

                _userRepository.UpdateUserInfo(user, modifiedByLogin);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string errorMessage)> UpdateUserPasswordAsync(string login, string newPassword, string modifiedByLogin)
        {
            //TODO: request old password for extra verification
            try
            {
                var result = CheckAccess(login, modifiedByLogin);
                if (!result.success)
                {
                    return (false, result.errorMessage);
                }
                _userRepository.UpdateUserPassword(login, newPassword, modifiedByLogin);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string errorMessage)> UpdateUserLoginAsync(string login, string newLogin, string modifiedByLogin)
        {
            try
            {
                var result = CheckAccess(login, modifiedByLogin);
                if (!result.success)
                {
                    return (false, result.errorMessage);
                }
                _userRepository.UpdateUserLogin(login, newLogin, modifiedByLogin);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(List<User> users, bool success, string errorMessage)> GetAllActiveUsersAsync()
        {
            try
            {
                var users = _userRepository.GetAllActiveUsers();
                return (users, true, null);
            }
            catch (Exception ex)
            {
                return (null, false, ex.Message);
            }
        }

        public async Task<(User user, bool success, string errorMessage)> GetUserByLoginAndPasswordAsync(string login, string password, string modifiedByLogin)
        {
            try
            {
                if (modifiedByLogin == login)
                {
                    User modUser = _userRepository.GetUserByLogin(modifiedByLogin);
                    if (modUser != null)
                    {
                        if (modUser.RevokedOn == null)
                        {
                            var user = _userRepository.GetUserByLogin(login);
                            if (user != null)
                            {
                                if (user.Password == password)
                                {
                                    return (user, true, null);
                                }
                            }
                            return (null, false, "User not found");
                        }
                    }
                }
                return (null, false, "Not authorized");
            }
            catch (Exception ex)
            {
                return (null, false, ex.Message);
            }
        }

        public async Task<(List<User> users, bool success, string errorMessage)> GetUsersOlderThanAsync(int age)
        {
            try
            {
                var users = _userRepository.GetUsersOlderThan(age);
                return (users, true, null);
            }
            catch (Exception ex)
            {
                return (null, false, ex.Message);
            }
        }

        public async Task<(bool success, string errorMessage)> DeleteUserAsync(string login, bool softDelete, string revokedByLogin)
        {
            try
            {
                _userRepository.DeleteUser(login, softDelete, revokedByLogin);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string errorMessage)> RestoreUserAsync(string login, string modifiedByLogin)
        {
            try
            {
                _userRepository.RestoreUser(login, modifiedByLogin);
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }


        public async Task<bool> ValidateUserAsync(string login, string password)
        {
            try
            {
                var user = _userRepository.GetUserByLogin(login);
                if (user != null)
                {
                    if (user.Password == password)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<(string role, bool success, string errorMessage)> GetUserRoleAsync(string login)
        {

            User user = _userRepository.GetUserByLogin(login);
            if (user != null)
            {

                return (user.Admin ? "Admin" : "User", true, null);
            }
            else
            {
                return (null, false, $"User with login:{login} not found");
            }
        }
    }
}
