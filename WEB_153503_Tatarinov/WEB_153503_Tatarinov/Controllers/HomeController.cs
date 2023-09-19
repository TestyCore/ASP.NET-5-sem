using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB_153503_Tatarinov.Models;

namespace WEB_153503_Tatarinov.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Header"] = "Lab 2";
        
        var list = new List<ListDemo>
        {
            new ListDemo { Id = 1, Name = "Item 1" },
            new ListDemo { Id = 2, Name = "Item 2" },
            new ListDemo { Id = 3, Name = "Item 3" }
        };
        
        return View(list);
    }
}