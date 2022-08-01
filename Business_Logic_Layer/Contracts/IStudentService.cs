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
        Task<List<StudentViewModel>> GetAllStudentsAsync();
        Task<StudentViewModel> GetStudentByIdAsync(int id);
        Task<int> AddStudentAsync(StudentViewModel student);
        Task UpdateStudentAsync(int id, StudentViewModel student);
        Task DeleteStudentAsync(int id);
        Task<List<StudentViewModel>> GetStudentsByNameAsync(string name);
        Task<StudentViewModel> GetStudentCoursesByIdAsync(int id);
        Task EnrollStudentInACourseAsync(int StudentId, int CourseId);
    }
}
