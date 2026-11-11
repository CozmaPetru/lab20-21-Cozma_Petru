using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibrarieLab.Models;

namespace LibrarieLab.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        string id = HttpContext.TraceIdentifier;
        if (Activity.Current != null)
        {
            id = Activity.Current.Id;
        }
        return View(new ErrorViewModel { RequestId = id });
    }
}
