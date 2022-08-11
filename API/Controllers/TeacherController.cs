using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Exceptions;
using Service.Contracts;
using System;
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

        [Authorize(Roles = "HR, Manager Academics")]
        [HttpGet("{id}/course")]
        public async Task<IActionResult> GetTeacherCoursesById([FromRoute]int id)
        {
            var res = await _teacherService.GetTeacherCoursesByIdAsync(id);

            if(res == null)
            {
                return BadRequest($"No such teacher with Teacher ID: {id}");
            }

            return Ok(res);
        }

        [Authorize(Policy = "Trusted"/*, Roles = "Manager Academics"*/)]
        [HttpPost("{id}/course")]
        public async Task<IActionResult> AssignTeacherInACourse([FromRoute]int id, [FromForm]int CourseId)
        {
            try
            {
                await _teacherService.AssignTeacherInACourseAsync(id, CourseId);
            }
            catch(CourseNotPresentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(StudentOrTeacherNotEnrolledException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction("GetTeacherCoursesById", new { id = id }, new { id, CourseId });
        }
    }
}
