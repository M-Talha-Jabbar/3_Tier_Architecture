using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Service.ViewModels;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(Roles = "Manager Academics")] // Controller is secured but if you want an action method in it to be access by anyone then simply put [AllowAnonymous] attribute on it.
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllStudents()
        {
            var res = await _studentService.GetAllStudentsAsync();

            if(res.Count == 0)
            {
                return Ok("Currently there are no students");
            }

            return Ok(res);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute]int id)
        {
            var res = await _studentService.GetStudentByIdAsync(id);

            if(res == null)
            {
                return BadRequest($"No such student with Student ID: {id}");
            }

            return Ok(res);
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetStudentsByName([FromQuery]string name)
        {
            var res = await _studentService.GetStudentsByNameAsync(name);

            if(res.Count == 0)
            {
                return BadRequest($"No such student with Student Name: {name}");
            }

            return Ok(res);
        }

        [AllowAnonymous]
        [HttpGet("{id}/course")]
        public async Task<IActionResult> GetStudentCoursesById([FromRoute]int id)
        {
            var res = await _studentService.GetStudentCoursesByIdAsync(id);

            if(res == null)
            {
                return BadRequest($"No such student with Student ID: {id}");
            }

            return Ok(res);
        }

        [HttpPost("{id}/course")]
        public async Task<IActionResult> EnrollStudentInACourse([FromRoute]int id, [FromForm]int CourseId)
        {
            var res = await _studentService.EnrollStudentInACourseAsync(id, CourseId);

            return CreatedAtAction("GetStudentCoursesById", new { id = id }, new { res });
        }
    }
}
