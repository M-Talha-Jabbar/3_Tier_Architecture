using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        [Required] [MaxLength(20)] public string CourseName { get; set; }
    }
}
