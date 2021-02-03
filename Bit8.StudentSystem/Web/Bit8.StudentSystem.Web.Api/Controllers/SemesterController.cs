using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bit8.StudentSystem.Data.TransferModels;
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
        public IActionResult Get(int id)
        {
            var result = this.semesterService.GetById(id);
            return new JsonResult(result);
        }

        // POST api/<SemesterController>
        [HttpPost]
        public IActionResult Post([FromBody] SemesterCreateModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name) || !this.CheckDate( model.StartDate.ToString() ) || !this.CheckDate(model.EndDate.ToString()))
            {
                return BadRequest(new { message = "Bad parameters passed!" });
            }

            foreach (var discipline in model.Disciplines)
            {
                if (discipline == null || string.IsNullOrWhiteSpace(discipline.DisciplineName) || string.IsNullOrWhiteSpace(discipline.ProfessorName))
                {
                    return BadRequest(new { message = "Bad discipline parameters passed!" });
                }
            }
            
            this.semesterService.Create(model);
            return Ok(new { message = "Successfully created." });
        }

        // PUT api/<SemesterController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SemesterEditModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name))
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            this.semesterService.Edit(id, model);
            return Ok(new { message = "Successfully updated." });
        }

        // DELETE api/<SemesterController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
