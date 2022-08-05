using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels
{
    public class TeacherViewModel
    {
        public int TeacherId { get; set; }
        [Required] [MaxLength(20)] public string TeacherName { get; set; }
        public List<CourseViewModel> Courses { get; set; }
    }
}
