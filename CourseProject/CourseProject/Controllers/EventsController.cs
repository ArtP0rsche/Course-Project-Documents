using CourseProject.Data;
using CourseProject.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CourseProject.Controllers
{
    public class EventsController : Controller
    {
        private readonly EmploymentServiceContext _context;

        public EventsController(EmploymentServiceContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Title,Description,AvailableSpace,UpdatedOn,EventDate,Status")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.UpdatedOn = DateOnly.FromDateTime(DateTime.Now);
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        [HttpPost, ActionName("ExportToXlsx")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportToXlsxAsync()
        {
            ExcelPackage.License.SetNonCommercialPersonal("CProj");

            List<Event> events = await _context.Events.Where(e => e.EventDate.Month == DateTime.Now.Month).ToListAsync();

            string filePath = @$"C:\Users\Артём\Documents\{DateTime.Now.ToString("MMMM yyyy HH.mm.ss")}.xlsx";

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.Add($"{DateTime.Now.ToString("MMMM yyyy")}");

                worksheet.Cells[1, 1].Value = "№ п/п";
                SetStyle(worksheet, 1, 1);
                worksheet.Cells[1, 2].Value = "Мероприятие";
                SetStyle(worksheet, 1, 2);
                worksheet.Cells[1, 3].Value = "Описание";
                SetStyle(worksheet, 1, 3);
                worksheet.Cells[1, 4].Value = "Количество мест";
                SetStyle(worksheet, 1, 4);
                worksheet.Cells[1, 5].Value = "Статус";
                SetStyle(worksheet, 1, 5);
                worksheet.Cells[1, 6].Value = "Дата проведения";
                SetStyle(worksheet, 1, 6);

                for (int i = 0; i < events.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = i + 1;
                    SetStyle(worksheet, i + 2, 1);
                    worksheet.Cells[i + 2, 2].Value = events[i].Title;
                    SetStyle(worksheet, i + 2, 2);
                    worksheet.Cells[i + 2, 3].Value = events[i].Description;
                    worksheet.Cells[i + 2, 3].Style.WrapText = true;
                    SetStyle(worksheet, i + 2, 3);
                    worksheet.Cells[i + 2, 4].Value = events[i].AvailableSpace;
                    SetStyle(worksheet, i + 2, 4);
                    worksheet.Cells[i + 2, 5].Value = events[i].Status;
                    SetStyle(worksheet, i + 2, 5);
                    worksheet.Cells[i + 2, 6].Value = events[i].EventDate;
                    SetStyle(worksheet, i + 2, 6);
                }

                worksheet.Cells.AutoFitColumns();
                worksheet.Column(3).Width = 60;
                worksheet.Column(6).Style.Numberformat.Format = "dd.MM.yyyy HH:mm:ss";

                package.Save();
            }

            return RedirectToAction(nameof(Index));
        }

        public void SetStyle(ExcelWorksheet worksheet, int row, int column)
        {
            worksheet.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[row, column].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[row, column].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Title,Description,AvailableSpace,EventDate,Status")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    @event.UpdatedOn = DateOnly.FromDateTime(DateTime.Now);
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
