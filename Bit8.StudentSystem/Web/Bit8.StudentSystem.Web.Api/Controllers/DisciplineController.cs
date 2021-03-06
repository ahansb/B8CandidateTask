﻿using Bit8.StudentSystem.Data.TransferModels;
using Bit8.StudentSystem.Services.Data.Interfaces;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bit8.StudentSystem.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplineController : BaseController
    {
        private readonly IDisciplineService disciplineService;

        public DisciplineController(IDisciplineService disciplineService)
        {
            this.disciplineService = disciplineService;
        }

        // GET: api/<DisciplineController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = this.disciplineService.GetAll();
            return new JsonResult(new { Data = result });
        }

        // GET api/<DisciplineController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = this.disciplineService.GetById(id);
            return new JsonResult(result);
        }

        // POST api/<DisciplineController>
        [HttpPost]
        public IActionResult Post([FromBody] DisciplineCreateModel model)
        {
            if (!this.Validator.ValidateObject(model)
                || !this.Validator.ValidateRequiredStringProperty(model.DisciplineName)
                || !this.Validator.ValidateRequiredStringProperty(model.ProfessorName)
                || !this.Validator.ValidateId(model.SemesterId))
            {
                return BadRequest(new { message = "Bad parameters passed!" });
            }

            this.disciplineService.Create(model);
            return Ok(new { message = "Successfully updated." });
        }

        // PUT api/<DisciplineController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] DisciplineEditModel model)
        {
            if (!this.Validator.ValidateObject(model)
               || !this.Validator.ValidateRequiredStringProperty(model.ProfessorName))
            {
                return BadRequest(new { message = "Bad parameters passed!" });
            }

            var affectedRows = this.disciplineService.Edit(id, model.ProfessorName);
            return this.BuildNonQueryResponse(affectedRows);
        }

        // DELETE api/<DisciplineController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!this.Validator.ValidateId(id))
            {
                return BadRequest(new { message = "Bad parameters passed!" });
            }

            var affectedRows = this.disciplineService.Delete(id);
            return this.BuildNonQueryResponse(affectedRows);
        }
    }
}
