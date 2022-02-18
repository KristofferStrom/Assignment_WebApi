using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment.WebApi.Models.Entities
{
    public class ProductEntity
    {
        public ProductEntity(int id, string name, string? description, decimal price, int categoryId)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
        }

        public ProductEntity(string name, string? description, decimal price, int categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }
    }
}
