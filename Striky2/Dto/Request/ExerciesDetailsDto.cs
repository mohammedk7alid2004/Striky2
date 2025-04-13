namespace Striky2.Dto.Request
{
    public class ExerciesDetailsDto
    {
        public string? Name { get; set; }


        public string? Description { get; set; }

        public int? Count { get; set; }

        public int Douration { get; set; }
        public int ExerciseId { get; set; }
        public byte[]? VideoData { get; set; }

    }
}
