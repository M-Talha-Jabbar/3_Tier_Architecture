using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.Exceptions;
using Service.Contracts;
using Service.ViewModels;
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

        // Pagination (using Page-based Pagination(aka Offset-based Pagination))
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public IActionResult GetAllTeachers([FromQuery]PagedRequest pagedRequest)
        {
            var (metadata, list) = _teacherService.GetAllTeachersAsync(pagedRequest.PageNumber, pagedRequest.PageSize);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            if(list.Count == 0)
            {
                return Ok("No more teachers data");
            }

            return Ok(list);
        }

        [Authorize(Roles = "HR, Manager Academics")] // Either this action method can be access by the Identity with role HR OR Manager Academics.
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
        // Mentioning Policy and Role together means AND operator between them and if you do this that means an Identity which fulfills the 'Trusted' Policy & have role Manager Academics can only access this particular action method.
        // But lets say you want to make this AND to OR. How would you do that? So for this you have to Customize Policy Requirement using RequireAssertion() (to define our customized logic in order to satisfy business requirement) inside AddPolicy() when registering the policy.
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
