using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class AdminIndexModel : PageModel
{
    public string Username { get; set; } = "";

    public void OnGet()
    {
        Username = User.Identity?.Name ?? "Unknown User";
    }
}
