namespace Assignment.WebApi.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            
        }
        public ProductModel(int id, string name, string description, decimal price, string category)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }
        public ProductModel(string name, string description, decimal price, string category)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
