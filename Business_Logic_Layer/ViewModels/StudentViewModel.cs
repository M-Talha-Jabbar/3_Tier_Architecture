using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Service.ViewModels
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        [Required] [MaxLength(20)] public string StudentName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public List<CourseViewModel> Courses { get; set; }
        // JsonIgnore prevents the property from being serialized or deserialized.
    }
}
