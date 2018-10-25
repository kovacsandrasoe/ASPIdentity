using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP_07.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ASP_07.Data;

namespace ASP_07.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public HomeController(RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext dbContext )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = dbContext;

            
        }

        public async Task<IActionResult> Admin2()
        {
            var firstuser = userManager.Users.FirstOrDefault();
            await userManager.AddToRoleAsync(firstuser, "Admins");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Admin()
        {
            IdentityRole adminrole = new IdentityRole()
            {
                Name = "Admins"
            };
            await roleManager.CreateAsync(adminrole);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admins")]
        public IActionResult AllTodo()
        {
            return View();
        }

        public IActionResult Index()
        {
            var myself = this.User;
            var id = userManager.GetUserId(myself);

            var q = from x in dbContext.Todos
                    where x.OwnerId == id
                    select x;

            return View(q);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(TodoModel model)
        {
            //model <- user
            var myself = this.User;
            var id = userManager.GetUserId(myself);
            model.OwnerId = id;
            dbContext.Todos.Add(model);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            var myself = this.User;
            var id = userManager.GetUserId(myself);

            //mi a saját nevünk?
            string name = userManager.Users.Where(x => x.Id == id).
                Select(s => s.UserName).FirstOrDefault();

            return View("About", name);
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
