namespace Assignment.WebApi.Models
{
    public class CreateCategoryModel
    {
        public CreateCategoryModel(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
