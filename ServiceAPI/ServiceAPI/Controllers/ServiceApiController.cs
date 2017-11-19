using Microsoft.AspNetCore.Mvc;
using ServiceAPI.Dal;
using ServiceAPI.Dtos;
using ServiceAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Microsoft.Extensions.Options;

namespace ServiceAPI.Controllers
{
    [Route("api")]
    public class ServiceApiController : Controller
    {
        static readonly object setupLock = new object ();
        static readonly SemaphoreSlim parallelism = new SemaphoreSlim(2);

        [HttpGet("setup")]
        public IActionResult SetupDatabase()
        {
            lock (setupLock)
            {
                using (var context = new ApplicationDbContext())
                {
                    // Create database
                    context.Database.EnsureCreated();
                }
                return (Ok("database created"));
            }
        }
    }
}
