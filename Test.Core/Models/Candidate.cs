using System.ComponentModel.DataAnnotations;

namespace Test.Core.Models
{
    public class Candidate
    {
        [Key]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? Interval { get; set; }

        public string? LinkedInUrl { get; set; }

        public string? GitHubUrl { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
