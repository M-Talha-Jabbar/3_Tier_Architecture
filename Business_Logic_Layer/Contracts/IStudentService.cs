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
        List<StudentViewModel> GetAll();
        StudentViewModel GetById(int id);
        void Insert(StudentViewModel student);
        void Update(StudentViewModel student);
        void Delete(int id);
        List<StudentViewModel> GetStudentsByName(string name);
    }
}
