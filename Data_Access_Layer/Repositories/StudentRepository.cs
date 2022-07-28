using Repository.Contracts;
using Repository.Data;
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

        public List<Student> GetStudentsByName(string Name)
        {
            return _context.Students.Where(stud => stud.StudentName == Name).ToList();
        }
    }
}
