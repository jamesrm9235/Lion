using System.ComponentModel.DataAnnotations;

namespace Lion.Abstractions
{
    public class LionOptions
    {
        [Required]
        public string ConnectionString { get; set; }
    }
}
