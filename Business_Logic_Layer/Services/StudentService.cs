using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Repository.Contracts;
using Repository.Exceptions;
using Repository.Models;
using Service.Contracts;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Services
{
    // Using Redis Cache with 1 Key (where Key can contain N number of records) - aka Project Based Implementation of Redis Cache
    /*
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IDistributedCache _distributedCache;
        private string KeyName = "Master";

        public StudentService(IStudentRepository studentRepository, IDistributedCache distributedCache)
        {
            _studentRepository = studentRepository;
            _distributedCache = distributedCache;
        }

        // With SetAsync() & GetAsync() Redis Cache Functions
        //public async Task<List<StudentViewModel>> GetAllStudentsAsync()
        //{
        //    var EncodedList = await _distributedCache.GetAsync(KeyName);

        //    // First checking if data is present in Redis Cache
        //    if(EncodedList != null) // if it is true than that means Key with name 'Master' is present in our Redis Cache.
        //    {
        //        var DeserializeList = Encoding.UTF8.GetString(EncodedList);

        //        var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(DeserializeList);

        //        return students;
        //    }

        //    else
        //    {
        //        var res = await _studentRepository.GetAllAsync(); // Since this return an Task so you can't directly chain LINQ until promise is resolved and you get the result.

        //        var students = res.Select(stud => new StudentViewModel()
        //        {
        //            StudentId = stud.StudentId,
        //            StudentName = stud.StudentName,
        //        }).ToList();

        //        // Setting/Inserting data in Redis Cache
        //        var SerializeList = JsonConvert.SerializeObject(students);
        //        EncodedList = Encoding.UTF8.GetBytes(SerializeList);

        //        var option = new DistributedCacheEntryOptions()
        //                                .SetSlidingExpiration(TimeSpan.FromMinutes(20))
        //                                .SetAbsoluteExpiration(TimeSpan.FromHours(6));

        //        await _distributedCache.SetAsync(KeyName, EncodedList, option);

        //        return students;
        //    }
        //}
        

        // With SetStringAsync() & GetStringAsync() Redis Cache Functions
        public async Task<List<StudentViewModel>> GetAllStudentsAsync()
        {
            var DeserializeList = await _distributedCache.GetStringAsync(KeyName);

            // First checking if data is present in Redis Cache
            if (DeserializeList != null) // if it is true than that means Key with name 'Master' is present in our Redis Cache.
            { 
                var students = JsonConvert.DeserializeObject<List<StudentViewModel>>(DeserializeList);

                return students;
            }

            else
            {
                var res = await _studentRepository.GetAllAsync(); // Since this return an Task so you can't directly chain LINQ until promise is resolved and you get the result.

                var students = res.Select(stud => new StudentViewModel()
                {
                    StudentId = stud.StudentId,
                    StudentName = stud.StudentName,
                }).ToList();

                // Setting/Inserting data in Redis Cache
                var SerializeList = JsonConvert.SerializeObject(students);

                var option = new DistributedCacheEntryOptions()
                                        .SetSlidingExpiration(TimeSpan.FromMinutes(20))
                                        .SetAbsoluteExpiration(TimeSpan.FromHours(6));

                await _distributedCache.SetStringAsync(KeyName, SerializeList, option);

                return students;
            }
        }

        public async Task RefreshSlidingExpirationTimeInRedisCache()
        {
            await _distributedCache.RefreshAsync(KeyName); // Reset Sliding Expiration Time if Key is present in Redis Cache
        }

        public async Task<StudentViewModel> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if(student == null)
            {
                return null;
            }

            var studentData = new StudentViewModel()
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
            };

            return studentData;
        }

        public async Task<int> AddStudentAsync(StudentViewModel student)
        {
            var newStudent = new Student()
            {
                StudentName = student.StudentName,
            };

            _studentRepository.Insert(newStudent);

            await _studentRepository.SaveAsync();

            await _distributedCache.RemoveAsync(KeyName); // Removing the old students data from Redis Cache b/c we have added a new student in DB now, so, students data in Redis Cache should get updated.

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

            await _distributedCache.RemoveAsync(KeyName); // Removing the old students data from Redis Cache b/c we have updated a student in DB now, so, students data in Redis Cache should get updated too.
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteAsync(id);

            await _studentRepository.SaveAsync();

            await _distributedCache.RemoveAsync(KeyName); // Removing the old students data from Redis Cache b/c we have deleted a student in DB now, so, students data in Redis Cache should get updated.
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

            if(res == null)
            {
                return null;
            }

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

        public async Task<string> EnrollStudentInACourseAsync(int StudentId, int CourseId)
        {
            try
            {
                await _studentRepository.RegisterACourseAsync(StudentId, CourseId);
            }
            catch (CourseNotPresentException ex)
            {
                return ex.Message;
            }
            catch(StudentOrTeacherNotEnrolledException ex)
            {
                return ex.Message;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            await _studentRepository.SaveAsync();

            return $"Student with Id {StudentId} has been enrolled in Course with Id {CourseId}";
        }
    }
    */


    // User Based Implementation of Redis Cache (where there will be a unique key for each student in Redis Cache for caching each student data)
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IDistributedCache _distributedCache;

        public StudentService(IStudentRepository studentRepository, IDistributedCache distributedCache)
        {
            _studentRepository = studentRepository;
            _distributedCache = distributedCache;
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

        public async Task RefreshSlidingExpirationTimeInRedisCache(int id)
        {
            await _distributedCache.RefreshAsync($"StudentID:{id}"); // Reset Sliding Expiration Time if Key is present in Redis Cache
        }

        public async Task<StudentViewModel> GetStudentByIdAsync(int id)
        {
            var DeserializeString = await _distributedCache.GetStringAsync($"StudentID:{id}");

            // First checking if data is present in Redis Cache
            if (DeserializeString != null) // if it is true than that means Key with name 'Master' is present in our Redis Cache.
            {
                var studentData = JsonConvert.DeserializeObject<StudentViewModel>(DeserializeString);

                return studentData;
            }

            else
            {
                var student = await _studentRepository.GetByIdAsync(id);

                if (student == null)
                {
                    return null;
                }

                var studentData = new StudentViewModel()
                {
                    StudentId = student.StudentId,
                    StudentName = student.StudentName,
                };

                // Setting/Inserting data in Redis Cache
                var SerializeString = JsonConvert.SerializeObject(studentData);

                var option = new DistributedCacheEntryOptions()
                                        .SetSlidingExpiration(TimeSpan.FromMinutes(20)) // Cached object expires if it not being requested for a defined amount of time period.
                                        .SetAbsoluteExpiration(TimeSpan.FromHours(6)); // Expiration time of the cached object.
                                        // Note that Sliding Expiration should always be set lower than the Absolute Expiration.

                await _distributedCache.SetStringAsync($"StudentID:{id}", SerializeString, option);

                return studentData;
            }
        }

        public async Task<int> AddStudentAsync(StudentViewModel student)
        {
            var newStudent = new Student()
            {
                StudentName = student.StudentName,
            };

            _studentRepository.Insert(newStudent);

            await _studentRepository.SaveAsync();

            // Here we does not to remove any data from cache b/c we are adding new student and since we are doing user implementation of redis cache (i.e. each student will have a unique key in redis cache).

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

            await _distributedCache.RemoveAsync($"StudentID:{id}");
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _studentRepository.DeleteAsync(id);

            await _studentRepository.SaveAsync();

            await _distributedCache.RemoveAsync($"StudentID:{id}");
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

            if (res == null)
            {
                return null;
            }

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

        public async Task<string> EnrollStudentInACourseAsync(int StudentId, int CourseId)
        {
            try
            {
                await _studentRepository.RegisterACourseAsync(StudentId, CourseId);
            }
            catch (CourseNotPresentException ex)
            {
                return ex.Message;
            }
            catch (StudentOrTeacherNotEnrolledException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            await _studentRepository.SaveAsync();

            return $"Student with Id {StudentId} has been enrolled in Course with Id {CourseId}";
        }
    }
}
