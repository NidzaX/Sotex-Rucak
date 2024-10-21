using System.Text.Json.Serialization;

namespace Sotex.Api.Model
{
    public class SideDish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid MenuId { get; set; }

        [JsonIgnore]
        public Menu Menu { get; set; }
    }
}
