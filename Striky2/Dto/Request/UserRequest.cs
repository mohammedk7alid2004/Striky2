namespace Striky2.Dto.Request
{
    public class UserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty ;
        public IFormFile? Photo { get; set; } 
        public int? Age { get; set; }
        public int Height { get; set; }
        public decimal? Weight { get; set; }

        public string? FitnessLevel { get; set; }

        public string? Goal { get; set; }
        public string phone { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;


    }
}
