namespace Striky.Api.Dto
{
    public class CategoryDto
    {
        public String Name { get; set; }
        public IFormFile Photo { get; set; }

        public int CountExercises { get; set; }
    }
}
