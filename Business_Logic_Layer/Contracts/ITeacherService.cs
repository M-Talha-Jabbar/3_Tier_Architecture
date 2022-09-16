using Service.Models;
using Service.Services;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ITeacherService
    {
        Task AssignTeacherInACourseAsync(int TeacherId, int CourseId);
        Task<TeacherViewModel> GetTeacherCoursesByIdAsync(int id);
        (object, List<TeacherViewModel>) GetAllTeachersAsync(int pageNumber, int pageSize);
    }
}
