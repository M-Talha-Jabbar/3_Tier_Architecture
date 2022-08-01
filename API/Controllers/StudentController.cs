using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.ViewModels;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllStudents()
        {
            return Ok(await _studentService.GetAllStudentsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute]int id)
        {
            return Ok(await _studentService.GetStudentByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody]StudentViewModel student)
        {
            var studentId = await _studentService.AddStudentAsync(student);

            return CreatedAtAction("GetStudentById", new { id = studentId }, new { studentId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute]int id, [FromBody]StudentViewModel student)
        {
            await _studentService.UpdateStudentAsync(id, student);

            return Ok("Student Updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute]int id)
        {
            await _studentService.DeleteStudentAsync(id);

            return Ok("Student Deleted Successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentsByName([FromQuery]string name)
        {
            return Ok(await _studentService.GetStudentsByNameAsync(name));
        }

        [HttpGet("{id}/course")]
        public async Task<IActionResult> GetStudentCoursesById([FromRoute]int id)
        {
            return Ok(await _studentService.GetStudentCoursesByIdAsync(id));
        }

        [HttpPost("{id}/course")]
        public async Task<IActionResult> EnrollStudentInACourse([FromRoute]int id, [FromForm]int CourseId)
        {
            await _studentService.EnrollStudentInACourseAsync(id, CourseId);

            return CreatedAtAction("GetStudentCoursesById", new { id = id }, new { id, CourseId });
        }
    }
}
