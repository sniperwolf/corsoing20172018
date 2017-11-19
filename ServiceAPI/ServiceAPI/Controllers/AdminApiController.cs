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
    public class AdminController : Controller
    {
        private IAdminService _adminService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AdminController(
            IAdminService adminService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _adminService = adminService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        static readonly object setupLock = new object ();
        static readonly SemaphoreSlim parallelism = new SemaphoreSlim(2);

        [HttpGet("admins")]
        public async Task<IActionResult> GetAdmins()
        {
            try
            {
                await parallelism.WaitAsync();

                var admins = _adminService.GetAll();
                var adminDtos = _mapper.Map<IList<AdminDto> >(admins);

                return (Ok(adminDtos));
            }
            finally
            {
                parallelism.Release();
            }
        }

        [HttpGet("admins/{id}")]
        public async Task<IActionResult> GetAdmin(int id)
        {
            try
            {
                await parallelism.WaitAsync();

                var admin = _adminService.GetById(id);
                var adminDto = _mapper.Map<AdminDto>(admin);

                return (Ok(adminDto));
            }
            finally
            {
                parallelism.Release();
            }
        }

        [AllowAnonymous]
        [HttpPost("admins")]
        public IActionResult Create([FromBody] AdminDto adminDto)
        {
            // map dto to entity
            var admin = _mapper.Map<Admin>(adminDto);

            try
            {
                // save
                _adminService.Create(admin, adminDto.Password);
                return (Ok());
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return (BadRequest(ex.Message));
            }
        }

        [HttpPut("admins/{id}")]
        public IActionResult Update(int id, [FromBody] AdminDto adminDto)
        {
            // map dto to entity and set id
            var admin = _mapper.Map<Admin>(adminDto);

            admin.Id = id;

            try
            {
                // save
                _adminService.Update(admin, adminDto.Password);
                return (Ok());
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return (BadRequest(ex.Message));
            }
        }

        [HttpDelete("admins/{id}")]
        public IActionResult Delete(int id)
        {
            _adminService.Delete(id);
            return (Ok());
        }
    }
}
