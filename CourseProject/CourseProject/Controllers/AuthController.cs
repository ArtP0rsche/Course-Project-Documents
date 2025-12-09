using CourseProject.Data;
using CourseProject.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly EmploymentServiceContext _context;

        public AuthController(EmploymentServiceContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind("Username,Password")] User @user)
        {
            var currentUser = _context.Users.FirstOrDefault(u => u.Username == @user.Username);

            if (currentUser != null && currentUser.Password == user.Password)
            {
                HttpContext.Session.SetString("UserRole", _context.Roles.Where(r => r.RoleId == currentUser.RoleId).Select(r => r.Name).FirstOrDefault());
                HttpContext.Session.SetString("UserFullname", $"{currentUser.Surname} {currentUser.Name} {currentUser.Patronymic}");
                HttpContext.Session.SetString("UserId", currentUser.UserId.ToString());
                HttpContext.Session.SetString("UserName", currentUser.Name);
                HttpContext.Session.SetString("UserSurname", currentUser.Surname);

                return RedirectToAction("Index", "Events");
            }
            else
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Events");
        }
    }
}
