using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeAutomationWeb.DAL;
using HomeAutomationWeb.Models;

namespace HomeAutomationWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            var dataAccess = new CSVfileHandler();

            ViewData["SensorsList"] = dataAccess.GetAllSensors();
        }
    }
}