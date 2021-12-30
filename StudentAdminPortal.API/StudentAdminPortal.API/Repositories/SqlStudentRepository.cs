using StudentAdminPortal.API.DataModels;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Repositories
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly StudentAdminContext context;
        private Guid studentId;

        public SqlStudentRepository(StudentAdminContext context)
        {
            this.context = context;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await context.Student.Include(nameof(Gender)).Include(nameof(Address)).ToListAsync();
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            return await context.Student.Include(nameof(Gender)).Include(nameof(Address))
                .FirstOrDefaultAsync(x => x.Id == studentId);
        }

        public async Task<List<Gender>> GetGendersAsync()
        {
            return await context.Gender.ToListAsync();
        }

        public async Task<bool> Exists(Guid studentId)
        {
            return await context.Student.AnyAsync(x => x.Id == studentId);
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student requestToUpdate)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if(existingStudent != null)
            {
                existingStudent.FirstName = requestToUpdate.FirstName;
                existingStudent.LastName = requestToUpdate.LastName;
                existingStudent.DateOfBirth = requestToUpdate.DateOfBirth;
                existingStudent.Email = requestToUpdate.Email;
                existingStudent.Mobile = requestToUpdate.Mobile;
                existingStudent.GenderId = requestToUpdate.GenderId;
                existingStudent.Address.PhysicalAddress = requestToUpdate.Address.PhysicalAddress;
                existingStudent.Address.PostalAddress = requestToUpdate.Address.PostalAddress;

                await context.SaveChangesAsync();

                return existingStudent;
            }
            return null;
        }

        public async Task<Student> DeleteStudent(Guid studentId)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if( existingStudent != null)
            {
                context.Student.Remove(existingStudent);
                await context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }

        public async Task<Student> CreateStudent(Student requestToCreate)
        {
            var studentToCreate = await context.Student.AddAsync(requestToCreate);
            await context.SaveChangesAsync();
            return studentToCreate.Entity;
        }
    }
}
