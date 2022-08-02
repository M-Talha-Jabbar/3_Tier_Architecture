using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.Models;
using Service.Contracts;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var res = await _teacherRepository.GetQuerable()
                                            .Where(t => t.TeacherId == id)
                                            .Include(t => t.Courses)
                                            .FirstOrDefaultAsync();

            var teacherCourses = new TeacherViewModel()
            {
                TeacherId = res.TeacherId,
                TeacherName = res.TeacherName,
                Courses = res.Courses.Select(c => new CourseViewModel()
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

            var teacher = await _teacherRepository.GetQuerable()
                                            .Where(t => t.TeacherId == TeacherId)
                                            .FirstOrDefaultAsync();

            teacher.Courses.Add(course);

            // we can't do 
            // _teacherRepository.GetQuerable().Update(teacher); // b/c GetQuerable() has converted the DbSet type into IQuerable type and Update() is the method of DbSet type.

            _teacherRepository.Update(teacher);

            await _teacherRepository.SaveAsync();
        }
    }
}
