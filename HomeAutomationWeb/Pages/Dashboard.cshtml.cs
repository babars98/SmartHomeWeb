using HomeAutomationWeb.DAL;
using HomeAutomationWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeAutomationWeb.Pages
{
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
            var data = new DataAccess().GetSensorData(null).OrderBy(c => c.Time).ToList();
            ViewData["TempData"] = data;
        }
    }
}
