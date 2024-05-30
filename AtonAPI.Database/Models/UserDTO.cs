namespace AtonAPI.Data.Models
{
    public class UserDTO
    {
        public string Name { get; set; }
        /// <summary>
        /// 0 - female, 1 - male, 2 - unknown
        /// </summary>
        public int Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsActive { get; set; }
    }
}
