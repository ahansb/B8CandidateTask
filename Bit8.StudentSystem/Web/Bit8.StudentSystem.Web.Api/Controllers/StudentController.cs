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
    public class StudentController : ControllerBase
    {
        private readonly IStudentService service;

        public StudentController(IStudentService studentService)
        {
            this.service = studentService;
        }

        // GET: api/<StudentController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = this.service.GetAll();
            return new JsonResult(new { Data = result });
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            var result = this.service.GetById(id);
            return new JsonResult(result);
        }

        // POST api/<StudentController>
        [HttpPost]
        public IActionResult Post([FromBody] StudentCreateModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Surname))
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            this.service.Create(model);
            return Ok(new { message = "Successfully created." });
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // DELETE api/<StudentController>/5/semester/3
        [HttpDelete("{id}/semester/{semesterId}")]
        public IActionResult DeleteStudentSemester(int id, int semesterId)
        {
            if (id <= 0 || semesterId <= 0)
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            this.service.DeleteStudentSemester(id, semesterId);
            return Ok(new { message = "Successfully deleted." });
        }

        // POST api/<StudentController>/5/semester
        [HttpPost("{id}/semester")]
        public IActionResult AddStudentSemester(int id, [FromBody] StudentSemesterCreateModel model)
        {
            if (id <= 0 || model == null || model.Id <= 0)
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            this.service.AddStudentSemester(id, model);
            return Ok(new { message = "Successfully created." });
        }

        // POST api/<StudentController>/5/disciplineScore
        [HttpPost("{id}/disciplineScore")]
        public IActionResult SetStudentScore(int id, [FromBody] StudentDisciplineScore model)
        {
            if (id <= 0 || model == null || model.DisciplineId <= 0 || model.Score <= 0)
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            this.service.AddStudentDisciplineScore(id, model);
            return Ok(new { message = "Successfully created." });
        }

        // PUT api/<StudentController>/5/disciplineScore
        [HttpPut("{id}/disciplineScore")]
        public IActionResult EditStudentScore(int id, [FromBody] StudentDisciplineScore model)
        {
            if (id <= 0 || model == null || model.DisciplineId <= 0 || model.Score <= 0)
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            this.service.EditStudentDisciplineScore(id, model);
            return Ok(new { message = "Successfully created." });
        }

        // PUT api/<StudentController>/5/disciplineScore
        [HttpDelete("{id}/disciplineScore")]
        public IActionResult DeleteStudentScore(int id, [FromBody] DeleteStudentDisciplineScore model)
        {
            if (id <= 0 || model == null || model.DisciplineId <= 0)
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            this.service.DeleteStudentDisciplineScore(id, model);
            return Ok(new { message = "Successfully created." });
        }
    }
}
