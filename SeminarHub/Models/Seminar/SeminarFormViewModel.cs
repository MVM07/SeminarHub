using SeminarHub.Models.Category;
using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.ValidationConstants.ValidationConstants;

namespace SeminarHub.Models.Seminar
{
    public class SeminarFormViewModel
    {
        [Required]
        [StringLength(SeminarTopicMaxLength,
            MinimumLength = SeminarTopicMinLength)]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarLecturerMaxLength, 
            MinimumLength = SeminarLecturerMinLength)]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [StringLength(SeminarDetailsMaxLength,
            MinimumLength = SeminarDetailsMinLength)]
        public string Details { get; set; } = string.Empty;

        [Required]
        public string DateAndTime { get; set; } = string.Empty;

        [Range(SeminarDurationMinLength, SeminarDurationMaxLength,
            ErrorMessage = "Duration is outside of the boundaries.")]
        public int Duration { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public IEnumerable<CategoryViewModel?> Categories { get; set; } = new List<CategoryViewModel>();
    }
}


