using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bit8.StudentSystem.Services.Data.Interfaces;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bit8.StudentSystem.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService semesterService;

        public SemesterController(ISemesterService semesterService)
        {
            this.semesterService = semesterService;
        }

        // GET: api/<SemesterController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = this.semesterService.GetAll();
            return new JsonResult(new { Data = result });
        }

        // GET api/<SemesterController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SemesterController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SemesterController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SemesterController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
