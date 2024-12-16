namespace SeminarHub.Models.Seminar
{
    public class SeminarAllViewModel
    {
        public int Id { get; set; }

        public string Topic { get; set; } = string.Empty;

        public string Lecturer { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public DateTime DateAndTime { get; set; }

        public string Organizer { get; set; } = string.Empty;
    }
}
