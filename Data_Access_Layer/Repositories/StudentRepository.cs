using Microsoft.EntityFrameworkCore;
using Repository.Contracts;
using Repository.Data;
using Repository.Exceptions;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolDBContext context) : base(context)
        {
        }

        public async Task<List<Student>> GetStudentsByNameAsync(string Name)
        {
            return await _context.Students
                                .Where(stud => stud.StudentName == Name)
                                .ToListAsync();
        }

        public async Task<Student> GetStudentCoursesByIdAsync(int id)
        {
            return await _context.Students
                                .Where(stud => stud.StudentId == id)
                                .Include(stud => stud.Courses)
                                .FirstOrDefaultAsync();
        }

        public async Task RegisterACourseAsync(int StudentId, int CourseId)
        {
            var course = await _context.Courses.FindAsync(CourseId);

            if(course == null)
            {
                throw new CourseNotPresentException();
            }

            var student = await _context.Students
                                    .Where(stud => stud.StudentId == StudentId)
                                    .Include(stud => stud.Courses)
                                    .FirstOrDefaultAsync();

            if(student == null)
            {
                throw new StudentOrTeacherNotEnrolledException();
            }

            student.Courses.Add(course);

            // _context.Students.Update(student);
            // OR
            Update(student); 
        }
    }
}
