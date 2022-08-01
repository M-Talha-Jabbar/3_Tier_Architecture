using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Contracts
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<List<Student>> GetStudentsByNameAsync(string Name);
        Task<Student> GetStudentCoursesByIdAsync(int id);
        Task RegisterACourseAsync(int StudentId, int CourseId);
    }
}
