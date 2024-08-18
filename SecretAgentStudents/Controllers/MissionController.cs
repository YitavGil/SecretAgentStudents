using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecretAgentStudents.Controllers
{
    [Authorize]
    public class MissionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
