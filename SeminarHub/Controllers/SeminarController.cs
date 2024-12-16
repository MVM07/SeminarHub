using Microsoft.AspNetCore.Mvc;
using SeminarHub.Contracts;
using SeminarHub.Data.ValidationConstants;
using SeminarHub.Models.Seminar;
using System.Globalization;

namespace SeminarHub.Controllers
{
    public class SeminarController : BaseController
    {
        private readonly ISeminarService seminarService;

        public SeminarController(ISeminarService _seminarService)
        {
            seminarService = _seminarService;
        }

        public async Task<IActionResult> All()
        {
            var seminars = await seminarService.GetAllSeminarsAsync();

            return View(seminars);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            SeminarFormViewModel model = new SeminarFormViewModel()
            {
                Categories = await seminarService.GetCategoriesAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeminarFormViewModel model)
        {           
            if (!ModelState.IsValid)
            {
                model.Categories = await seminarService.GetCategoriesAsync();

                return View(model);
            }

            string currentUserId = GetCurrentUserId();

            await seminarService.AddSeminarAsync(model, currentUserId);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Joined(int id)
        {
            string currentUserId = GetCurrentUserId();

            var seminars = await seminarService.GetJoinedSeminarsAsync(currentUserId);

            return View(seminars);
        }

        public async Task<IActionResult> Join(int id)
        {
            string currentUserId = GetCurrentUserId();

            await seminarService.JoinSeminarAsync(currentUserId, id);

            return RedirectToAction(nameof(Joined));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string currentUserId = GetCurrentUserId();

            var categoryToEdit = await seminarService.GetSeminarToEditAsync(currentUserId, id);

            return View(categoryToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SeminarFormViewModel model, int id)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await seminarService.GetCategoriesAsync();

                return View(model);
            }

            string currentUserId = GetCurrentUserId();

            await seminarService.EditEventAsync(model, id, currentUserId);

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Leave(int id)
        {
            string currentUserId = GetCurrentUserId();

            await seminarService.LeaveSeminarAsync(currentUserId, id);

            return RedirectToAction(nameof(Joined));
        }

        public async Task<IActionResult> Details(int id)
        {
            string currentUserId = GetCurrentUserId();

            var details = await seminarService.GetSeminarDetailsAsync(id);

            return View(details);
        }

        public async Task<IActionResult> Delete(int id)
        {
            string currentUserId = GetCurrentUserId();

            await seminarService.DeleteSeminarAsync(currentUserId, id);

            return RedirectToAction(nameof(All));
        }
    }
}
