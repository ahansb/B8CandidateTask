using Bit8.StudentSystem.Services.Data.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Bit8.StudentSystem.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregatedDataController : BaseController
    {
        private readonly IAggregatedDataService service;

        public AggregatedDataController(IAggregatedDataService service)
        {
            this.service = service;
        }

        [HttpGet("[action]")]
        public IActionResult GetTopStudents()
        {
            var result = this.service.GetTopTenStudents();
            return new JsonResult(new { Data = result });
        }

        [HttpGet("[action]")]
        public IActionResult GetNoScoresStudents()
        {
            var result = this.service.GetNoScoresStudents();
            return new JsonResult(new { Data = result });
        }
    }
}
