using AtonAPI.Data;
using AtonAPI.Data.Models;

namespace AtonAPI.Services
{
    public interface IUserService
    {
        Task<(UserDTO userDTO, bool success, string errorMessage)> GetUserDTOByLoginAsync(string login);
        Task<(bool success, string errorMessage)> CreateUserAsync(UserCreateModel userCreateModel, string createdByLogin);
        Task<(bool success, string errorMessage)> UpdateUserInfoAsync(string login, string modifiedByLogin, string? name = null, int? gender = null, DateTime? birthDay = null);
        Task<(bool success, string errorMessage)> UpdateUserPasswordAsync(string login, string newPassword, string modifiedByLogin);
        Task<(bool success, string errorMessage)> UpdateUserLoginAsync(string login, string newLogin, string modifiedByLogin);
        Task<(List<User> users, bool success, string errorMessage)> GetAllActiveUsersAsync();
        Task<(User user, bool success, string errorMessage)> GetUserByLoginAndPasswordAsync(string login, string password, string modifiedByLogin);
        Task<(List<User> users, bool success, string errorMessage)> GetUsersOlderThanAsync(int age);
        Task<(bool success, string errorMessage)> DeleteUserAsync(string login, bool softDelete, string revokedBy);
        Task<(bool success, string errorMessage)> RestoreUserAsync(string login, string modifiedByLogin);
        Task<bool> ValidateUserAsync(string login, string password);
        Task<(string role, bool success, string errorMessage)> GetUserRoleAsync(string login);
    }
}
