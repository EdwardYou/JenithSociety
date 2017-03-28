using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZenithWebSite.Data;
using ZenithWebSite.Models.JenithEventModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace ZenithWebSite.Controllers
{
    [Produces("application/json")]
    [Route("api/ActivityAPI")]
    [EnableCors("MyPolicy")]
    public class ActivityAPIController : Controller
    {
        private ApplicationDbContext _context;

        public ActivityAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/ActivityAPI
        [Authorize(Roles = "Admin, Member")]
        [HttpGet]
        public IEnumerable<Activity> Get()
        {
            return _context.Activities.ToList(); ;
        }

        // GET: api/ActivityAPI/5
        [Authorize(Roles = "Admin, Member")]
        [HttpGet("{id}", Name = "GetActivity")]
        public Activity Get(int id)
        {
            return _context.Activities.FirstOrDefault(a => a.ActivityId == id);
        }

        // POST: api/ActivityAPI
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Post([FromBody]Activity value)
        {
            _context.Activities.Update(value);
            _context.SaveChanges();
        }

        // PUT: api/ActivityAPI/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Activity value)
        {
            _context.Activities.Update(value);
            _context.SaveChanges();
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var value = _context.Activities.FirstOrDefault(a => a.ActivityId == id);
            if (value != null)
            {
                _context.Activities.Remove(value);
                _context.SaveChanges();
            }
        }
    }
}
