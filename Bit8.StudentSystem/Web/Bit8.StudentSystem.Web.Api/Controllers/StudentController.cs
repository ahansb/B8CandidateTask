using Bit8.StudentSystem.Data.TransferModels;
using Bit8.StudentSystem.Services.Data.Interfaces;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bit8.StudentSystem.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseController
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
            if (!this.Validator.ValidateObject(model)
                || !this.Validator.ValidateRequiredStringProperty(model.Name)
                || !this.Validator.ValidateRequiredStringProperty(model.Surname)
                || !this.Validator.ValidateObject(model.Semesters))
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            foreach (var semesterId in model.Semesters)
            {
                if (this.Validator.ValidateId(semesterId))
                {
                    return BadRequest(new { message = "Bad discipline parameters passed!" });
                }
            }

            this.service.Create(model);
            return Ok(new { message = "Successfully created." });
        }

        //TODO: Do I need this?
        //// PUT api/<StudentController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<StudentController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        // DELETE api/<StudentController>/5/semester/3
        [HttpDelete("{id}/semester/{semesterId}")]
        public IActionResult DeleteStudentSemester(int id, int semesterId)
        {
            if (!this.Validator.ValidateId(id) || !this.Validator.ValidateId(semesterId))
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
            if (!this.Validator.ValidateId(id)
                || !this.Validator.ValidateObject(model)
                || !this.Validator.ValidateId(model.Id))
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
            if (!this.Validator.ValidateId(id)
                || !this.Validator.ValidateObject(model)
                || !this.Validator.ValidateId(model.DisciplineId)
                || !this.Validator.ValidateScore(model.Score))
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
            if (!this.Validator.ValidateId(id)
                || !this.Validator.ValidateObject(model)
                || !this.Validator.ValidateId(model.DisciplineId)
                || !this.Validator.ValidateScore(model.Score))
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
            if (!this.Validator.ValidateId(id)
                || !this.Validator.ValidateObject(model)
                || !this.Validator.ValidateId(model.DisciplineId))
            {
                return BadRequest(new { message = "Bad discipline parameters passed!" });
            }

            this.service.DeleteStudentDisciplineScore(id, model);
            return Ok(new { message = "Successfully created." });
        }
    }
}
