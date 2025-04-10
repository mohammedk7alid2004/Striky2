namespace Striky2.Dto.Request
{
    public class ExerciseRequestDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } =string.Empty;
        public IFormFile Photo { get; set; }
        public int? Count { get; set; }
        public int? Duration { get; set; }
        
    }
}
