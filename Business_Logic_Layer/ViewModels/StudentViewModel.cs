using Repository.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        [Required] [MaxLength(20)] public string StudentName { get; set; }
        public List<CourseViewModel> Courses { get; set; }
    }
}
