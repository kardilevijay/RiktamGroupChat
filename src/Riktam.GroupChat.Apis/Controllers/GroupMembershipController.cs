using Microsoft.AspNetCore.Mvc;

namespace Riktam.GroupChat.Apis.Controllers;

public class GroupMembershipController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
