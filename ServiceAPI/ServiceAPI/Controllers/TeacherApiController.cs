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
using Microsoft.AspNetCore.Authorization;
using ServiceAPI.Entities;

namespace ServiceAPI.Controllers
{
    [Authorize]
    [Route("api")]
    public class TeacherController : Controller
    {
        private ITeacherService _teacherService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public TeacherController(
            ITeacherService teacherService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _teacherService = teacherService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        static readonly object setupLock = new object ();
        static readonly SemaphoreSlim parallelism = new SemaphoreSlim(2);

        [HttpGet("teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            try
            {
                await parallelism.WaitAsync();


                var teachers = _teacherService.GetAll();
                var teacherDtos = _mapper.Map<IList<TeacherDto> >(teachers);

                return (Ok(teacherDtos));
            }
            finally
            {
                parallelism.Release();
            }
        }

        [HttpGet("teachers/{id}")]
        public async Task<IActionResult> GetTeacher(int id)
        {
            try
            {
                await parallelism.WaitAsync();

                var teacher = _teacherService.GetById(id);
                var teacherDto = _mapper.Map<TeacherDto>(teacher);

                return (Ok(teacherDto));
            }
            finally
            {
                parallelism.Release();
            }
        }

        [AllowAnonymous]
        [HttpPost("teachers")]
        public IActionResult Create([FromBody] TeacherDto teacherDto)
        {
            // map dto to entity
            var teacher = _mapper.Map<Teacher>(teacherDto);

            try
            {
                // save
                _teacherService.Create(teacher, teacherDto.Password);
                return (Ok());
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return (BadRequest(ex.Message));
            }
        }

        /*[HttpGet("teacher")]
         * public async Task<IActionResult> GetTeacher([FromQuery] int id)
         * {
         *  using (var context = new ApplicationDbContext())
         *  {
         *      var teacherDto = _mapper.Map<TeacherDto>(await context.Teachers.FirstOrDefaultAsync(x => x.Id == id));
         *
         *      return Ok(teacherDto);
         *  }
         * }*/

        /*[HttpGet("teachers")]
         * public IActionResult GetAll()
         * {
         *  var teachers = _teacherService.GetAll();
         *  var teacherDtos = _mapper.Map<IList<TeacherDto>>(teachers);
         *  return Ok(teacherDtos);
         * }*/

        [HttpPut("teachers/{id}")]
        public IActionResult Update(int id, [FromBody] TeacherDto teacherDto)
        {
            // map dto to entity and set id
            var teacher = _mapper.Map<Teacher>(teacherDto);

            teacher.Id = id;

            try
            {
                // save
                _teacherService.Update(teacher, teacherDto.Password);
                return (Ok());
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return (BadRequest(ex.Message));
            }
        }

        [HttpDelete("teachers/{id}")]
        public IActionResult Delete(int id)
        {
            _teacherService.Delete(id);
            return (Ok());
        }
    }
}
