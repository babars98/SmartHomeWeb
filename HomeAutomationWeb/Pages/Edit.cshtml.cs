using HomeAutomationWeb.DAL;
using HomeAutomationWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeAutomationWeb.Pages
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Sensor _sensor { get; set; } = new Sensor();
        private readonly DataAccess _dataAccess = new DataAccess();

        public void OnGet(string sensorId)
        {
            _sensor = _dataAccess.GetSensorInfo(sensorId);
        }
        public IActionResult OnPost()
        {
            var dataAccess = new DataAccess();
            if (ModelState.IsValid)
            {
                var result = dataAccess.UpdateSensor(_sensor);
                if (result)
                    return RedirectToPage("/Index");
            }
            ModelState.AddModelError("", "An error while saving.");
            return Page();
        }
    }
}
