using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("{id}/course")]
        public async Task<IActionResult> GetTeacherCoursesById([FromRoute]int id)
        {
            return Ok(await _teacherService.GetTeacherCoursesByIdAsync(id));
        }

        [HttpPost("{id}/course")]
        public async Task<IActionResult> AssignTeacherInACourse([FromRoute]int id, [FromForm]int CourseId)
        {
            await _teacherService.AssignTeacherInACourseAsync(id, CourseId);

            return CreatedAtAction("GetTeacherCoursesById", new { id = id }, new { id, CourseId });
        }
    }
}
