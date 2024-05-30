namespace AtonAPI.Data.Models
{
    public class UserCreateModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 0 - female, 1 - male, 2 - unknown
        /// </summary>
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsAdmin { get; set; }
    }
}
