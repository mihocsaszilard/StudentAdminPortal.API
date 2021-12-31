using StudentAdminPortal.API.DataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();

        Task<Student> GetStudentAsync(Guid studentId);

        Task<List<Gender>> GetGendersAsync();

        Task<bool> Exists(Guid studentId);

        Task<Student> UpdateStudent(Guid studentId, Student requestToUpdate);

        Task<Student> DeleteStudent(Guid studentId);

        Task<Student> CreateStudent(Student requestToCreate);
        Task<bool> UpdateProfileImg(Guid studentId, string profimeImgUrl);
    }
}
