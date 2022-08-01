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

        public async Task<List<StudentCoursesViewModel>> GetAll()
        {
            var students = await _studentRepository.GetAll().Select(stud => new StudentCoursesViewModel()
            {
                StudentId = stud.StudentId,
                StudentName = stud.StudentName,
                Courses = stud.Courses,
            }).ToListAsync();

            return students;
        }

        public async Task<StudentCoursesViewModel> GetById(int id)
        {
            var student = await _studentRepository.GetById(id);

            var studentCourses = new StudentCoursesViewModel()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Courses = student.Courses
            };

            return studentCourses;
        }

        public async Task Insert(StudentCoursesViewModel student)
        {
            var newStudent = new Student()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Courses = student.Courses
            };

            _studentRepository.Insert(newStudent);

            await _studentRepository.SaveAsync();
        }

        public async Task Update(StudentCoursesViewModel student)
        {
            var updateStudent = new Student()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Courses = student.Courses
            };

            _studentRepository.Update(updateStudent);

            await _studentRepository.SaveAsync();
        }

        public async Task Delete(int id)
        {
            await _studentRepository.Delete(id);

            await _studentRepository.SaveAsync();
        }

        public async Task<List<StudentCoursesViewModel>> GetStudentsByName(string name)
        {
            var students = await _studentRepository.GetStudentsByName(name).Select(stud => new StudentCoursesViewModel()
            {
                StudentId = stud.StudentId,
                StudentName = stud.StudentName,
                Courses = stud.Courses
            }).ToListAsync();

            return students;
        }
    }
}
