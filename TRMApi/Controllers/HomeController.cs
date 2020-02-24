using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRMApi.Models;

namespace TRMApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger 
            , RoleManager<IdentityRole> roleManager
            , UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            //string[] roles = { "Manager", "Admin", "Cashier" };

            //foreach(var role in roles)
            //{
            //    var roleExists = await _roleManager.RoleExistsAsync(role);

            //    if (!roleExists)
            //    {
            //        await _roleManager.CreateAsync(new IdentityRole(role));
            //    }
            //}

            //var user = await _userManager.FindByEmailAsync("test@ubork.com");

            //if (user != null)
            //{
            //    await _userManager.AddToRoleAsync(user, "Admin");
            //    await _userManager.AddToRoleAsync(user, "Cashier");
            //    await _userManager.AddToRoleAsync(user, "Manager");
            //}

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
