﻿using System;
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
    public class DisciplineController : ControllerBase
    {
        private readonly IDisciplineService disciplineService;

        public DisciplineController(IDisciplineService disciplineService)
        {
            this.disciplineService = disciplineService;
        }

        // GET: api/<DisciplineController>
        [HttpGet]
        public JsonResult Get()
        {
            var result = this.disciplineService.GetAll();
            return new JsonResult(result);
        }

        // GET api/<DisciplineController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DisciplineController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DisciplineController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DisciplineController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
