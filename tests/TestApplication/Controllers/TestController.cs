using FluentValidation.HttpExtensions.TestInfrastructure;
using Microsoft.AspNetCore.Mvc;

namespace TestApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(TestEntity entity) => Ok();
    }
}
