using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        [Required] [MaxLength(20)] public string CourseName { get; set; }
    }
}
