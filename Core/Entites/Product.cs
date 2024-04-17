using System.ComponentModel.DataAnnotations;

namespace Core.Entites
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
