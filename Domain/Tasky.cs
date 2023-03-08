using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Tasky
    {
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        // public User owner {get; set;}
        // public Guid ProjectId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Importance { get; set; } = string.Empty;
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}