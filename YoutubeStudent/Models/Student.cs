using System.ComponentModel.DataAnnotations;

namespace YoutubeStudent.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string StudentName { get; set; }
        public int age { get; set; }
        public string Address { get; set; } = String.Empty;
    }
}
