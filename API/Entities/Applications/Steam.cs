using System.ComponentModel.DataAnnotations;

namespace API.Entities.Applications
{
    public class Steam
    {
        [Key]
        public int Uid { get; set; }
    }
}