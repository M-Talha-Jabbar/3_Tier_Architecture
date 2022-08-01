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
        Task<List<StudentCoursesViewModel>> GetAll();
        Task<StudentCoursesViewModel> GetById(int id);
        Task Insert(StudentCoursesViewModel student);
        Task Update(StudentCoursesViewModel student);
        Task Delete(int id);
        Task<List<StudentCoursesViewModel>> GetStudentsByName(string name);
    }
}
