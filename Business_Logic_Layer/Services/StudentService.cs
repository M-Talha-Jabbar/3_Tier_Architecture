using Repository.Contracts;
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

        public List<StudentViewModel> GetAll()
        {
            var students = _studentRepository.GetAll().Select(stud => new StudentViewModel()
            {
                StudentId = stud.StudentId,
                StudentName = stud.StudentName,
                
            }).ToList();

            return students;
        }

        public StudentViewModel GetById(int id)
        {
            var student = _studentRepository.GetById(id);

            var studentModel = new StudentViewModel()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
            };

            return studentModel;
        }

        public void Insert(StudentViewModel student)
        {

        }

        public void Update(StudentViewModel student)
        {

        }

        public void Delete(int id)
        {

        }

        public List<StudentViewModel> GetStudentsByName(string name)
        {
            var students = _studentRepository.GetStudentsByName(name).Select(stud => new StudentViewModel()
            {
                StudentId = stud.StudentId,
                StudentName = stud.StudentName,

            }).ToList();

            return students;
        }
    }
}
