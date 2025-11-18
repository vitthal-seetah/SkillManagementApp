using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace skillManager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // Redirect based on role

            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Employee")
                {
                    return Redirect("/Dashboard");
                }

                if (role == "TeamLead")
                {
                    return Redirect("/teamleaddashboard");
                }

                if (role == "Admin" || role == "Manager")
                {
                    return Redirect("/ManagerDashboard");
                }
            }
            return Page();
        }
    }
}
