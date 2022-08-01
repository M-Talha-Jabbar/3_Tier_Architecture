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
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<StudentViewModel>> GetAllStudentsAsync()
        {
            var res = await _studentRepository.GetAllAsync(); // Since this return an Task so you can't directly chain LINQ until promise is resolved and you get the result.

            var students = res.Select(stud => new StudentViewModel()
            {
                StudentId = stud.StudentId,
                StudentName = stud.StudentName,
            }).ToList();

            return students;
        }

        public async Task<StudentViewModel> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            var studentCourses = new StudentViewModel()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
            };

            return studentCourses;
        }

        public async Task<int> AddStudentAsync(StudentViewModel student)
        {
            var newStudent = new Student()
            {
                StudentName = student.StudentName,
            };

            _studentRepository.Insert(newStudent);

            await _studentRepository.SaveAsync();

            return newStudent.StudentId;
        }

        public async Task UpdateStudentAsync(int id, StudentViewModel student)
        {
            var updateStudent = new Student()
            {
                StudentId = id,
                StudentName = student.StudentName,
            };

            _studentRepository.Update(updateStudent);

            await _studentRepository.SaveAsync();
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteAsync(id);

            await _studentRepository.SaveAsync();
        }

        public async Task<List<StudentViewModel>> GetStudentsByNameAsync(string name)
        {
            var res = await _studentRepository.GetStudentsByNameAsync(name);
            
            var students = res.Select(stud => new StudentViewModel()
            {
                StudentId = stud.StudentId,
                StudentName = stud.StudentName,
            }).ToList();

            return students;
        }

        public async Task<StudentViewModel> GetStudentCoursesByIdAsync(int id)
        {
            var res = await _studentRepository.GetStudentCoursesByIdAsync(id);

            var studentCourses = new StudentViewModel()
            {
                StudentId = res.StudentId,
                StudentName = res.StudentName,
                Courses = res.Courses.Select(c => new CourseViewModel()
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                }).ToList()
            };

            return studentCourses;
        }

        public async Task EnrollStudentInACourseAsync(int StudentId, int CourseId)
        {
            await _studentRepository.RegisterACourseAsync(StudentId, CourseId);

            await _studentRepository.SaveAsync();
        }
    }
}
