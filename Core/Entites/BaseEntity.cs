using System.ComponentModel.DataAnnotations;

namespace Core.Entites
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
