namespace Striky2.Dto.Response;


    public class UserResponse
{
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        String Password { get; set; } = string.Empty;
    public string? Photo { get; set; } 
    public int? Age { get; set; }
        public int Height { get; set; }
        public decimal? Weight { get; set; }

        public string? FitnessLevel { get; set; }

        public string? Goal { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

   
}


