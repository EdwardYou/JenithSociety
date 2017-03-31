using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZenithWebSite.Models.JenithEventModel;
using ZenithWebSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace ZenithWebSite.Controllers
{
    [Produces("application/json")]
    [Route("api/EventAPI")]
    [EnableCors("MyPolicy")]
    public class EventAPIController : Controller
    {
        private ApplicationDbContext _context;

        public EventAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/EventAPI
        [Authorize(Roles = "Admin, Member")]
        [HttpGet]
        public IActionResult Get()
        {
            var model = from c in _context.Events
                        join a in _context.Activities on c.ActivityId equals a.ActivityId
                        select new { c.EventId, c.EventDateTimeFrom, c.EventDateTimeTo, c.UserName, c.ActivityId, a.ActivityDescr };
            return Json(model);
        }

        // GET: api/EventAPI/5
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("{id}", Name = "GetEvent")]
        public Event Get(int id)
        {
            return _context.Events.FirstOrDefault(e => e.EventId == id);
        }

        // POST: api/EventAPI
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post([FromBody]Event value)
        {
            _context.Events.Update(value);
            _context.SaveChanges();
        }

        // PUT: api/EventAPI/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Event value)
        {
            _context.Events.Update(value);
            _context.SaveChanges();
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var value = _context.Events.FirstOrDefault(e => e.EventId == id);
            if (value != null)
            {
                _context.Events.Remove(value);
                _context.SaveChanges();
            }
        }
    }
}
