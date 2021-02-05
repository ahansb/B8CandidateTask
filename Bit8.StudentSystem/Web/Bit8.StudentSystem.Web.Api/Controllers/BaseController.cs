using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Bit8.StudentSystem.Web.Api.Helpers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bit8.StudentSystem.Web.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected ValidationHelper Validator { get { return new ValidationHelper(); } }

        protected IActionResult BuildNonQueryResponse(int affectedRows)
        {
            if (affectedRows == 0)
            {
                return BadRequest(new { message = "No rows were affected!" });
            }
            else
            {
                return Ok(new { message = "Operation succesful." });
            }
        }
    }
}
