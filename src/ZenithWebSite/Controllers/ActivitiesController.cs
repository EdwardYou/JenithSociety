using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZenithWebSite.Models.JenithEventModel;
using ZenithWebSite.Models;
using Microsoft.EntityFrameworkCore;
using ZenithWebSite.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace ZenithSociety.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Activities
        public IActionResult Index()
        {
            ViewData["CreateNew"] = "Create New";
            return View(_context.Activities.ToList());
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.SingleOrDefaultAsync(a => a.ActivityId == id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // GET: Activities/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var activity = await _context.Activities.SingleOrDefaultAsync(a => a.ActivityId == id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("ActivityId,ActivityDescr,CreationDate")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                activity.CreationDate = DateTime.Now;
                _context.Activities.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActivityId,ActivityDescr,CreationDate")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(activity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(activity);
        }
        // GET: Activities/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var activity = await _context.Activities.SingleOrDefaultAsync(a => a.ActivityId == id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities.SingleOrDefaultAsync(a => a.ActivityId == id);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
