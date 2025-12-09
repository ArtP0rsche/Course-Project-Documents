using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseProject.Data;
using CourseProject.DataModels;
using Microsoft.AspNetCore.Authorization;

namespace CourseProject.Controllers
{
    public class RequestsController : Controller
    {
        private readonly EmploymentServiceContext _context;

        public RequestsController(EmploymentServiceContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index()
        {
            var employmentServiceContext = _context.Requests.Include(r => r.Event).Include(r => r.User);
            ViewBag.StatusOptions = new List<string> { "В планах", "Отклонена", "Принята" };
            return View(await employmentServiceContext.ToListAsync());
        }

        // GET: Requests/Create
        public IActionResult Create(int? eventId, int? userId)
        {
            if (eventId == null || userId == null)
            {
                return BadRequest();
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,UserId,EventId,Content,Status,UpdatedOn,Institution,PeopleNumber")] Request request)
        {
            if (request.EventId != null && request.UserId != null)
            {
                request.UpdatedOn = DateOnly.FromDateTime(DateTime.Now);
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Events");
            }
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventId", request.EventId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", request.UserId);
            return View();
        }

        [HttpPost, ActionName("UpdateStatus")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
                return Json(new { success = false, message = "Не найдено" });

            request.Status = status;
            request.UpdatedOn = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                _context.Update(request);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequestId,UserId,EventId,Content,Status,UpdatedOn,Institution,PeopleNumber")] Request request)
        {
            if (id != request.RequestId)
            {
                return NotFound();
            }

            try
            {
                request.UpdatedOn = DateOnly.FromDateTime(DateTime.Now);
                _context.Update(request);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.RequestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Edit", "Users", new { id = Convert.ToInt32(HttpContext.Session.GetString("UserId")) });
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request != null)
            {
                _context.Requests.Remove(request);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", "Users", new { id = Convert.ToInt32(HttpContext.Session.GetString("UserId")) });
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.RequestId == id);
        }
    }
}
