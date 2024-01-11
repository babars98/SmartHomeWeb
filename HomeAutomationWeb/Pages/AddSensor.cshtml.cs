using HomeAutomationWeb.DAL;
using HomeAutomationWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeAutomationWeb.Pages
{
    public class AddSensorModel : PageModel
    {
        [BindProperty]
        public Sensor _sensor { get; set; } = new Sensor();

        public void OnGet(Sensor sensor)
        {
            _sensor = sensor;
        }

        public IActionResult OnPost()
        {
            var dataAccess = new CSVfileHandler();
            if (ModelState.IsValid)
            {
                var result = dataAccess.AddSensor(_sensor);
                if (result)
                    return RedirectToPage("/Index");
            }
            ModelState.AddModelError("", "An error while saving.");
            return Page();
        }
    }
}
