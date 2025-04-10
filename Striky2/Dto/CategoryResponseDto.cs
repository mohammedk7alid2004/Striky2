namespace Striky2.Dto;

public class CategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PhotoUrl { get; set; }  // Changed to string
    public int CountExercises { get; set; }
}
