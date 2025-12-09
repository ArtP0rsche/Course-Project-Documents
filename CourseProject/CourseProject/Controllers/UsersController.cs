using CourseProject.Data;
using CourseProject.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly EmploymentServiceContext _context;

        public UsersController(EmploymentServiceContext context)
        {
            _context = context;
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId,Username,Password,Name,Surname,Patronymic")] User user)
        {
            if (!UserExists(user.Username))
            {
                user.RoleId = 1;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ModelState.AddModelError("", "Имя пользователя уже занято");
                return View();
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Users.Where(u => u.UserId == id).Include(u => u.Requests).ThenInclude(r => r.Event).FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RoleId,Username,Password,Name,Surname,Patronymic")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (!UserExists(user.UserId, user.Username))
            {
                try
                {
                    HttpContext.Session.SetString("UserFullname", $"{user.Surname} {user.Name} {user.Patronymic}");
                    HttpContext.Session.SetString("UserName", user.Name);
                    HttpContext.Session.SetString("UserSurname", user.Surname);

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Edit");
            }
            else
            {
                ModelState.AddModelError("", "Имя пользователя уже занято");
                return View();
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private bool UserExists(string username)
        {
            return _context.Users.Any(e => e.Username == username);
        }

        private bool UserExists(int id, string username)
        {
            return _context.Users.Any(e => e.Username == username && e.UserId != id);
        }
    }
}
