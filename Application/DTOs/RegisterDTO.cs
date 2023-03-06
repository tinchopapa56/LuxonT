using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
        public static class rol {
            public static readonly string LEAD = "LEAD";
            public static readonly string MID = "MID";
            public static readonly string JR = "JR";
        }
    public class RegisterDTO
    {
        [Required(ErrorMessage = "The userName is required")]
        public string UserName  { get; set; }
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Pass must have: 1 min,1mayus,1number,1special, 8ch")]
        public string Password { get; set; }
        // public Rol Rol { get; set; }

        // [InRange(AllowableValues = new[] { "LEAD", "MID", "JR" }, ErrorMessage = "Rol must be either LEAD, MID or JR)")]
        // [InRange(new[] { "LEAD", "MID", "JR" }, ErrorMessage = "Rol must be either 'LEAD', 'MID', or 'JR'.")]
        [RegularExpression("^(LEAD|MID|JR)$", ErrorMessage = "Rol must be either 'LEAD', 'MID', or 'JR'.")]
        public string Rol { get; set; }
    }
}