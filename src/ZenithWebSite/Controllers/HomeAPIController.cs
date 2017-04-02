using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZenithWebSite.Data;
using ZenithWebSite.Models.JenithEventModel;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using ZenithWebSite.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using Microsoft.AspNetCore.Cors;

namespace ZenithWebSite.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/HomeAPI")]
    [EnableCors("MyPolicy")]
    public class HomeAPIController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private static int i;

        public HomeAPIController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/HomeAPI
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            DateTime startOfWeek = DateTime.Today.AddDays((int)(DateTime.Today.DayOfWeek) * -1 + -6);
            DateTime endOfWeek = DateTime.Today.AddDays((int)(DateTime.Today.DayOfWeek) * -1 + 1);

            var eventList = from e in _context.Events
                            join a in _context.Activities on e.ActivityId equals a.ActivityId
                            orderby (e.EventDate)
                            select new { e.EventId, e.EventDateTimeFrom, e.EventDateTimeTo, e.UserName, e.ActivityId, e.IsActive, e.EventDate, a.ActivityDescr };

            var curUsr = await _userManager.GetUserAsync(HttpContext.User);
            string role = "Annoymous";
            if (curUsr != null)
            {
                Debug.WriteLine("User is Not Null");
                var currRoles = await _userManager.GetRolesAsync(curUsr);

                foreach (var r in currRoles)
                {
                    if (r.Equals("Admin") || r.Equals("Member"))
                    {
                        role = "Members";
                    }
                }
                if (role.Equals("Members"))
                {
                    Debug.WriteLine("Members!!!!!!!!!!!!!");
                    return Json(eventList);
                }
                
            } else
            {
                var annonyMember = from el in eventList
                                   where (el.EventDateTimeFrom >= startOfWeek)
                                   && (el.EventDateTimeFrom < endOfWeek)
                                   && (el.IsActive == true)
                                   orderby (el.EventDate)
                                   select new { el.EventId, el.EventDateTimeFrom, el.EventDateTimeTo, el.UserName, el.ActivityId, el.ActivityDescr };

                Debug.WriteLine("User is Null");
                return Json(annonyMember);
            }

            return null;
        }
    }
}
