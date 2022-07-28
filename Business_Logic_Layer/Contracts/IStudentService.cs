using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IStudentService
    {
        List<StudentModel> GetAll();
        StudentModel GetById(int id);
        void Insert(StudentModel student);
        void Update(StudentModel student);
        void Delete(int id);
        List<StudentModel> GetStudentsByName(string name);
    }
}
