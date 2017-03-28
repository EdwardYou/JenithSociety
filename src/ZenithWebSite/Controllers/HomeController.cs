using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZenithWebSite.Data;
using ZenithWebSite.Models.JenithEventModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ZenithWebSite.Models;
using System.Diagnostics;

namespace ZenithWebSite.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private static int i;
        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public class RenderView
        {
            public Event Event { get; set; }
            public Activity Activity { get; set; }
        }
        public async Task<IActionResult> Index(string week = "current")
        {
            
            var curUsr = await _userManager.GetUserAsync(User);
            var currRole = await _userManager.GetRolesAsync(curUsr);
            ViewData["CurrRole"] = currRole;
            //ViewData["CurrRole"] = "Annonymouse";
            if (currRole.Equals("Admin") || currRole.Equals("Member"))
            {
                if (week.Equals("current"))
                {
                    i = 1;
                }
                else if (week.Equals("nextWeek"))
                {
                    i = i + 7;
                }
                else if (week.Equals("preWeek"))
                {
                    i = i - 7;
                }
            }
            else
            {
                i = 1;
            }

            DateTime startOfWeek = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1 + 1)+i);
            DateTime endOfWeek = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1 + 8)+i);

            var eventList = from e in _context.Events
                            join a in _context.Activities on e.ActivityId equals a.ActivityId
                            where (e.EventDateTimeFrom >= startOfWeek)
                            && (e.EventDateTimeFrom < endOfWeek)
                            && (e.IsActive == true)
                            orderby (e.EventDate)
                            select new RenderView { Event = e, Activity = a };
            return View(eventList);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
