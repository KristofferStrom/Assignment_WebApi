using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Assignment.WebApi.Models.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class CategoryEntity
    {
        public CategoryEntity()
        {
            
        }
        public CategoryEntity(string name)
        {
            Name = name;
        }
        public CategoryEntity(int id, string name)
        {
            Id = id;
            Name = name;
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public virtual ICollection<ProductEntity> Products { get; }
    }
}
