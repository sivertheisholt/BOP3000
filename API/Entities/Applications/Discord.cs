using System.ComponentModel.DataAnnotations;

namespace API.Entities.Applications
{
    public class Discord
    {
        [Key]
        public int Uid { get; set; }
    }
}