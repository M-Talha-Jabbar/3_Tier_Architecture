using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.Exceptions;
using Repository.Models;
using Service.Contracts;
using Service.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly IGenericRepository<Teacher> _teacherRepository;

        public TeacherService(IGenericRepository<Teacher> teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<TeacherViewModel> GetTeacherCoursesByIdAsync(int id)
        {
            var teacher = await _teacherRepository.GetQuerable()
                                            .Where(t => t.TeacherId == id)
                                            .Include(t => t.Courses)
                                            .FirstOrDefaultAsync();

            if(teacher == null)
            {
                return null;
            }

            var teacherCourses = new TeacherViewModel()
            {
                TeacherId = teacher.TeacherId,
                TeacherName = teacher.TeacherName,
                Courses = teacher.Courses.Select(c => new CourseViewModel()
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName
                }).ToList()
            };

            return teacherCourses;
        }

        public async Task AssignTeacherInACourseAsync(int TeacherId, int CourseId)
        {
            var course = await _teacherRepository.GetQueryable<Course>()
                                            .Where(c => c.CourseId == CourseId)
                                            .FirstOrDefaultAsync();

            if (course == null) throw new CourseNotPresentException();

            var teacher = await _teacherRepository.GetQuerable()
                                            .Where(t => t.TeacherId == TeacherId)
                                            .Include(t => t.Courses)
                                            .FirstOrDefaultAsync();

            if (teacher == null) throw new StudentOrTeacherNotEnrolledException();

            teacher.Courses.Add(course);

            // we can't do this:
            // _teacherRepository.GetQuerable().Update(teacher); // b/c GetQuerable() has converted the DbSet type into IQuerable type implicitly and Update() is the method of DbSet type.

            _teacherRepository.Update(teacher);

            await _teacherRepository.SaveAsync();
        }
    }
}
