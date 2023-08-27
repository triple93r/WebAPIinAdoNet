using System.ComponentModel.DataAnnotations;

namespace YoutubeStudent.Models
{
    public class StudentSemester
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Studid { get; set; }
        public string Subject1 { get; set; } = String.Empty;
        public int Mark1 { get; set; }
        public string Subject2 { get; set; } = String.Empty;
        public int Mark2 { get; set; }
        public int TotalMark { get; set; }
    }
}
