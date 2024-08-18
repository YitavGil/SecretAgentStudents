using Microsoft.AspNetCore.Mvc;
using SecretAgentStudents.Models;
using SecretAgentStudents.Services;

namespace SecretAgentStudents.Controllers
{
    public class AuthController : Controller
    {
        private readonly jwtService _jwtService;
        // מדמה לי את הדאטאבייס
        private static List<Agent> _agent = new List<Agent>();

        // DI
        public AuthController(jwtService jwtService)
        {
            _jwtService = jwtService;
        }

        // ננהל את ההרשמה של סוכן חדש
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Agent agent)
        {
            // אם  המשתמש כבר נרשם - תעיף אותו ללוג-אין
            if (ModelState.IsValid) { 
                _agent.Add(agent);
                return RedirectToAction("Login");
            }

            return View(agent);
        }

        // LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Agent agent)
        {
            var registeredAgent = _agent.FirstOrDefault(a => a.CodeName == agent.CodeName && a.Password == agent.Password);
            if (registeredAgent != null) 
            {
                var token = _jwtService.GenerateToken(agent.CodeName);

                // לשמור את הטוקן בעוגייה (קוקי)

                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                });

                return RedirectToAction("Index", "Mission");
            }

            ModelState.AddModelError(string.Empty, "נכשלת בהתחברות");

            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JwtToken");
            return RedirectToAction("Index", "Home");
        }

    }
}
