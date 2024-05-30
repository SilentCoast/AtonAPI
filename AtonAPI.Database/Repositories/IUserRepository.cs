namespace AtonAPI.Data.Repositories
{
    public interface IUserRepository
    {
        User GetUserByLogin(string login);

        void CreateUser(User user, string createdBy);

        void UpdateUserInfo(User user, string modifiedBy);

        void UpdateUserPassword(string login, string newPassword, string modifiedBy);

        void UpdateUserLogin(string login, string newLogin, string modifiedBy);

        List<User> GetAllActiveUsers();

        List<User> GetUsersOlderThan(int age);

        void DeleteUser(string login, bool softDelete, string revokedBy);

        void RestoreUser(string login, string modifiedBy);

    }
}
