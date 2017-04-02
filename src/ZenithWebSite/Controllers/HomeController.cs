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
using System.Globalization;

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
        
        public async Task<IActionResult> Index(string week = "current")
        {
            var curUsr = await _userManager.GetUserAsync(User);
            string role = "Annoymous";
            if (curUsr != null)
            {
                var currRoles = await _userManager.GetRolesAsync(curUsr);

                foreach (var r in currRoles)
                {
                    if (r.Equals("Admin") || r.Equals("Member"))
                    {
                        role = "Members";
                    }
                }
                ViewData["CurrRole"] = role;
                if (role.Equals("Members"))
                {
                    if (week.Equals("current"))
                    {
                        i = 0;
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
            } else
            {
                ViewData["CurrRole"] = role;
            }

            
            DateTime startOfWeek = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1 + 1)+i);
            DateTime endOfWeek = DateTime.Today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1 + 8)+i);

            ViewData["CurrWeekNum"] = getCurrWeekNum(startOfWeek);

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

        #region Helpers
        public class RenderView
        {
            public Event Event { get; set; }
            public Activity Activity { get; set; }
        }

        public string getCurrWeekNum(DateTime now)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            int weekNumber = ci.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            string yearweek = now.Year +"-" + weekNumber;
            return yearweek; 
        }
        #endregion
    }
}
