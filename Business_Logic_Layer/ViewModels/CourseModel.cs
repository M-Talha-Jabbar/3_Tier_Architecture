using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class CourseModel
    {
        [Required] public int CourseId { get; set; }
        [Required] public string CourseName { get; set; }
        public ICollection<StudentModel> Students { get; set; }
        public ICollection<TeacherModel> Teachers { get; set; }
    }
}
