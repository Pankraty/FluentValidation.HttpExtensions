using FluentValidation.HttpExtensions.TestInfrastructure;
using Microsoft.AspNetCore.Mvc;

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotFoundController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(NotFoundEntity entity) => Ok();
    }
}
