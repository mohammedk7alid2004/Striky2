namespace Striky2.Dto.Response
{
    public class Exercies_Response
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Photo { get; set; }
        public int? Count { get; set; }
        public int? Duration { get; set; }
    }
}
