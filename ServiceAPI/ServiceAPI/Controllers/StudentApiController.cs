using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using ServiceAPI.Services;
using AutoMapper;
using Microsoft.Extensions.Options;
using ServiceAPI.Helpers;
using ServiceAPI.Dtos;
using System.Collections.Generic;
using ServiceAPI.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ServiceAPI.Controllers
{
    [Authorize]
    [Route("api")]
    public class StudentController : Controller
    {
        private IStudentService _studentService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public StudentController(
            IStudentService studentService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _studentService = studentService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        static readonly object setupLock = new object ();
        static readonly SemaphoreSlim parallelism = new SemaphoreSlim(2);

        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                await parallelism.WaitAsync();


                var students = _studentService.GetAll();
                var studentDtos = _mapper.Map<IList<StudentDto> >(students);

                return (Ok(studentDtos));
            }
            finally
            {
                parallelism.Release();
            }
        }

        [HttpGet("students/{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                await parallelism.WaitAsync();

                var student = _studentService.GetById(id);
                var studentDto = _mapper.Map<StudentDto>(student);

                return (Ok(studentDto));
            }
            finally
            {
                parallelism.Release();
            }
        }

        [AllowAnonymous]
        [HttpPost("students")]
        public IActionResult Create([FromBody] StudentDto studentDto)
        {
            // map dto to entity
            var student = _mapper.Map<Student>(studentDto);

            try
            {
                // save
                _studentService.Create(student, studentDto.Password);
                return (Ok());
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return (BadRequest(ex.Message));
            }
        }

        [HttpPut("students/{id}")]
        public IActionResult Update(int id, [FromBody] StudentDto studentDto)
        {
            // map dto to entity and set id
            var student = _mapper.Map<Student>(studentDto);

            student.Id = id;

            try
            {
                // save
                _studentService.Update(student, studentDto.Password);
                return (Ok());
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return (BadRequest(ex.Message));
            }
        }

        [HttpDelete("students/{id}")]
        public IActionResult Delete(int id)
        {
            _studentService.Delete(id);
            return (Ok());
        }
    }
}
