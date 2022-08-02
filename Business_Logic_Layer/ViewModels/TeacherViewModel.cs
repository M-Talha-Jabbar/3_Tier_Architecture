using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class TeacherViewModel
    {
        public int TeacherId { get; set; }
        [Required] [MaxLength(20)] public string TeacherName { get; set; }
        public List<CourseViewModel> Courses { get; set; }
    }
}
