namespace Sotex.Api.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FitstName { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }

        public List<MenuItem> MenuItems { get; set; }
        public List<Order> Orders { get; set; }

    }
}
