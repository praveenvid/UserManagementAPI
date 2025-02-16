namespace UserManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool UserStatus { get; set; }
    }
}
