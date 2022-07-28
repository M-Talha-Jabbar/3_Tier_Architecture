using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class StudentModel
    {
        [Required] public int StudentId { get; set; }
        [Required] public string StudentName { get; set; }
        public ICollection<CourseModel> Courses { get; set; }
    }
}
