using Microsoft.AspNetCore.Mvc;

namespace prjDemoAjax.Controllers
{
    public class ApiController : Controller
    {
        public IActionResult Index()
        {
            return Content("Hello Ajax!! First Ajax Program");
        }
    }
}
