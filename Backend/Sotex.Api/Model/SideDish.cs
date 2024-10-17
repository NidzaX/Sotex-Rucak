namespace Sotex.Api.Model
{
    public class SideDish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
