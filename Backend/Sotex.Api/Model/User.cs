namespace Sotex.Api.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public List<Menu> Menus { get; set; } = new List<Menu>();
    }
}
