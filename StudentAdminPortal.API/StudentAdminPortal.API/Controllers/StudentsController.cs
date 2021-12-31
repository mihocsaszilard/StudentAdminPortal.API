using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAdminPortal.API.DomainModels;
using StudentAdminPortal.API.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StudentAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly IImageRepository imageRepository;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper, IImageRepository imageRepository)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.imageRepository = imageRepository;
        }

        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await studentRepository.GetStudentsAsync();

            return Ok(mapper.Map<List<Student>>(students));
        }

        [HttpGet]
        [Route("[controller]/{studentId:guid}"), ActionName("GetStudentAsync")]
        public async Task<IActionResult> GetStudentAsync([FromRoute] Guid studentId)
        {
            // Fetch single student data
            var student = await studentRepository.GetStudentAsync(studentId);

            // Return student data 
            if (student == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<Student>(student));
        }

        [HttpPut]
        [Route("[controller]/{studentId:guid}")]
        public async Task<IActionResult> UpdateStudentAsync([FromRoute] Guid studentId, [FromBody] UpdateStudentRequest request)
        {
            if (await studentRepository.Exists(studentId))
            {
                // Update details
                var updatedStudent = await studentRepository.UpdateStudent(studentId, mapper.Map<DataModels.Student>(request));

                if (updatedStudent != null)
                {
                    return Ok(mapper.Map<Student>(updatedStudent));
                }
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("[controller]/{studentId:guid}")]

        public async Task<IActionResult> DeleteStudentAsync([FromRoute] Guid studentId)
        {
            if (await studentRepository.Exists(studentId))
            {
                // Delete Student
                var deletedStudent = await studentRepository.DeleteStudent(studentId);

                if (deletedStudent != null)
                {
                    return Ok(mapper.Map<Student>(deletedStudent));
                }

            }
            return NotFound();
        }

        [HttpPost]
        [Route("[controller]/New")]
        public async Task<IActionResult> AddStudentAsync([FromBody] CreateStudentRequest request)
        {
            var createdStudent = await studentRepository.CreateStudent(mapper.Map<DataModels.Student>(request));
            return CreatedAtAction(nameof(GetStudentAsync), new { studentId = createdStudent.Id },
                mapper.Map<Student>(createdStudent));
        }

        [HttpPost]
        [Route("[controller]/{studentId:guid}/upload-img")]
        public async Task<IActionResult> UploadImg([FromRoute] Guid studentId, IFormFile profileImg)
        {

            var validExtensions = new List<string>
            {
                ".jpeg",
                ".jpg",
                ".png",
                ".gif"
            };

            if (profileImg != null && profileImg.Length > 0)
            {
                var extentsion = Path.GetExtension(profileImg.FileName);

                if (validExtensions.Contains(extentsion))
                {
                    // Check if student exists
                    if (await studentRepository.Exists(studentId))
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImg.FileName);

                        // Upload image to localStorage
                        var fileImgPath = await imageRepository.Upload(profileImg, fileName);

                        if (await studentRepository.UpdateProfileImg(studentId, fileImgPath))
                        {
                            return Ok(fileImgPath);
                        }
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image!");
                    }
                };
            }
            return BadRequest("Wrong image extension!");
        }

    }
}
