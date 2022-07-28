using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class TeacherModel
    {
        [Required] public int TeacherId { get; set; }
        [Required] public string TeacherName { get; set; }
        public ICollection<CourseModel> Courses { get; set; }
    }
}
