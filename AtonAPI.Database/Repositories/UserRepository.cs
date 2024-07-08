namespace AtonAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AtonDBContext _context;

        public UserRepository(AtonDBContext context)
        {
            _context = context;
        }

        public User GetUserByLogin(string login)
        {
            User user = _context.Users.FirstOrDefault(p => p.Login == login);
            return user;
        }

        public void CreateUser(User user, string createdBy)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Guid = Guid.NewGuid();
            user.CreatedOn = DateTime.UtcNow;
            user.CreatedBy = createdBy;

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUserInfo(User user, string modifiedBy)
        {
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            _context.Users.Update(user);

            _context.SaveChanges();
        }


        public void UpdateUserPassword(string login, string newPassword, string modifiedBy)
        {
            var user = GetUserByLogin(login);
            if (user == null) return;

            user.Password = newPassword;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UpdateUserLogin(string login, string newLogin, string modifiedBy)
        {
            var user = GetUserByLogin(login);
            if (user == null) return;

            var loginExists = _context.Users.Any(u => u.Login == newLogin);
            if (loginExists)
                throw new ArgumentException("Login already exists");

            user.Login = newLogin;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public List<User> GetAllActiveUsers()
        {
            return _context.Users
                .Where(u => u.RevokedOn == null)
                .OrderBy(u => u.CreatedOn)
                .ToList();
        }


        public List<User> GetUsersOlderThan(int age)
        {
            var dateThreshold = DateTime.UtcNow.AddYears(-age);
            return _context.Users
                .Where(u => u.Birthday <= dateThreshold)
                .ToList();
        }

        public void DeleteUser(string login, bool softDelete, string revokedBy)
        {
            var user = GetUserByLogin(login);
            if (user == null) return;

            if (softDelete)
            {
                user.RevokedOn = DateTime.UtcNow;
                user.RevokedBy = revokedBy;
                _context.Update(user);
            }
            else
            {
                _context.Users.Remove(user);
            }

            _context.SaveChanges();
        }

        public void RestoreUser(string login, string modifiedBy)
        {
            var user = GetUserByLogin(login);
            if (user == null) return;

            user.RevokedOn = null;
            user.RevokedBy = null;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;
            _context.Update(user);
            _context.SaveChanges();
        }


    }
}
