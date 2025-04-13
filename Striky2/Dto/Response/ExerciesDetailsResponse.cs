namespace Striky2.Dto.Response
{
    public class ExerciesDetailsResponse
    {
        public int ExerciseDetailId { get; set; }
        public string Name { get; set; } =string.Empty;
        public string Description { get; set; } = string.Empty;
        public int  Count { get; set; }
        public int Douration { get; set; } 
        public string Vedo { get; set; }  = string.Empty;
        public int? ExerciseId { get; set; }
    }
}
