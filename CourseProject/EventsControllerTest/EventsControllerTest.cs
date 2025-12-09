using CourseProject.Controllers;
using CourseProject.Data;
using CourseProject.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseProject.Tests
{
    public class EventsControllerTest
    {
        [Fact]
        public async Task CreateEvent_Success_RedirectsToIndex()
        {
            // Arrange: настраиваем In‑Memory контекст с Pomelo (имитация MySQL)
            var options = new DbContextOptionsBuilder<EmploymentServiceContext>()
                .UseMySql(ServerVersion.Parse("8.0.44-mysql"))
                .Options;

            using var context = new EmploymentServiceContext(options);
            var controller = new EventsController(context);

            // Обходим проверку анти‑фордж‑токена в тестах
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var validEvent = new Event
            {
                Title = "Конференция AI 2025",
                Description = "Международная конференция по ИИ",
                AvailableSpace = 20,
                EventDate = DateTime.Now.AddDays(7),
                UpdatedOn = DateOnly.FromDateTime(DateTime.Now),
                Status = "В планах"
            };

            // Act: вызываем метод контроллера
            var result = await controller.Create(validEvent);

            // Assert: проверяем редирект на Index
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Проверяем, что событие сохранилось в БД
            Assert.True(context.Events.Any(e => e.Title == "Конференция AI 2025"));
            var savedEvent = context.Events.First(e => e.Title == "Конференция AI 2025");
            Assert.NotNull(savedEvent.UpdatedOn);
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), savedEvent.UpdatedOn.Value);
        }
    }
}
