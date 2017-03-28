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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;

namespace ZenithSociety.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EventsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Activities
        public IActionResult Index()
        {
            var events = _context.Events.Include(db => db.Activity);
            return View(events);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.SingleOrDefaultAsync(e => e.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewBag.ActivityId = new SelectList(_context.Activities, "ActivityId", "ActivityDescr", @event.ActivityId);
            return View(@event);
        }

        // GET: Events/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var events = await _context.Events.SingleOrDefaultAsync(e => e.EventId == id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // GET: Activities/Create
        public async Task<ActionResult> Create()
        {
            ViewData["CurrUser"] = await _userManager.GetUserAsync(User);
            ViewData["Activities"] = new SelectList(_context.Activities, "ActivityId", "ActivityDescr");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("EventId,EventDate,EventDateTimeFrom,EventDateTimeTo,IsActive,UserName,CreationDate,ActivityId")] Event events)
        {
            if (ModelState.IsValid)
            {
                events.CreationDate = DateTime.Now;
                _context.Events.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(events);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventDate,EventDateTimeFrom,EventDateTimeTo,IsActive,UserName,CreationDate,ActivityId")] Event events)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(events).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(events);
        }
        // GET: Activities/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var events = await _context.Events.SingleOrDefaultAsync(e => e.EventId == id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var events = await _context.Events.SingleOrDefaultAsync(e => e.EventId == id);
            _context.Events.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
