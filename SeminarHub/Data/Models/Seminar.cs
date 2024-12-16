using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SeminarHub.Data.ValidationConstants.ValidationConstants;

namespace SeminarHub.Data.Models
{
    public class Seminar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(SeminarTopicMaxLength)]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [MaxLength(SeminarLecturerMaxLength)]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [MaxLength(SeminarDetailsMaxLength)]
        public string Details { get; set; } = string.Empty;

        [Required]
        public string OrganiserId { get; set; } = string.Empty;

        [ForeignKey(nameof(OrganiserId))]
        public IdentityUser Organiser { get; set; } = null!;

        [Required]
        public DateTime DateAndTime { get; set; }

        [MaxLength(SeminarDurationMaxLength)]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public IEnumerable<SeminarParticipant> SeminarsParticipants { get; set; } = new List<SeminarParticipant>();
    }
}
