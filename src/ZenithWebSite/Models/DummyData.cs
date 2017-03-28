using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenithWebSite.Data;
using ZenithWebSite.Models.JenithEventModel;

namespace ZenithWebSite.Models
{
    public class DummyData
    {
        public static void Initialize(ApplicationDbContext db)
        {
            GetActivities(db);
            GetEvents(db);
        }

        public static void GetActivities(ApplicationDbContext activityDb)
        {
            if (!activityDb.Activities.Any())
            {
                activityDb.Activities.Add(new Activity()
                {
                    ActivityDescr = "Senior's Golf Tournament",
                    CreationDate = DateTime.Now,
                });
                activityDb.Activities.Add(new Activity()
                {
                    ActivityDescr = "Leadership General Assembly Meeting",
                    CreationDate = DateTime.Now,
                });
                activityDb.Activities.Add(new Activity()
                {
                    ActivityDescr = "Youth Bowling Tournament",
                    CreationDate = DateTime.Now,
                });
                activityDb.Activities.Add(new Activity()
                {
                    ActivityDescr = "Young ladies cooking lessons",
                    CreationDate = DateTime.Now,
                });
                activityDb.SaveChanges();
            }
            
        }

        public static void GetEvents (ApplicationDbContext eventDb)
        {
            if (!eventDb.Events.Any())
            {
                eventDb.Events.Add(new Event()
                {
                    EventDate = new DateTime(2017, 03, 21),
                    EventDateTimeFrom = new DateTime(2017, 03, 21, 15, 00, 00),
                    EventDateTimeTo = new DateTime(2017, 03, 21, 17, 00, 00),
                    IsActive = true,
                    UserName = "a",
                    CreationDate = DateTime.Now,
                    ActivityId = eventDb.Activities.FirstOrDefault(a => a.ActivityDescr == "Senior's Golf Tournament").ActivityId
                });
                eventDb.Events.Add(new Event()
                {
                    EventDate = new DateTime(2017, 03, 22),
                    EventDateTimeFrom = new DateTime(2017, 03, 22, 15, 00, 00),
                    EventDateTimeTo = new DateTime(2017, 03, 22, 17, 00, 00),
                    IsActive = true,
                    UserName = "a",
                    CreationDate = DateTime.Now,
                    ActivityId = eventDb.Activities.FirstOrDefault(a => a.ActivityDescr == "Leadership General Assembly Meeting").ActivityId
                });
                eventDb.Events.Add(new Event()
                {
                    EventDate = new DateTime(2017, 03, 23),
                    EventDateTimeFrom = new DateTime(2017, 03, 23, 15, 00, 00),
                    EventDateTimeTo = new DateTime(2017, 03, 23, 17, 00, 00),
                    IsActive = true,
                    UserName = "a",
                    CreationDate = DateTime.Now,
                    ActivityId = eventDb.Activities.FirstOrDefault(a => a.ActivityDescr == "Youth Bowling Tournament").ActivityId
                });
                eventDb.Events.Add(new Event()
                {
                    EventDate = new DateTime(2017, 03, 24),
                    EventDateTimeFrom = new DateTime(2017, 03, 24, 15, 00, 00),
                    EventDateTimeTo = new DateTime(2017, 03, 24, 17, 00, 00),
                    IsActive = true,
                    UserName = "a",
                    CreationDate = DateTime.Now,
                    ActivityId = eventDb.Activities.FirstOrDefault(a => a.ActivityDescr == "Young ladies cooking lessons").ActivityId
                });
                eventDb.Events.Add(new Event()
                {
                    EventDate = new DateTime(2017, 03, 25),
                    EventDateTimeFrom = new DateTime(2017, 03, 25, 15, 00, 00),
                    EventDateTimeTo = new DateTime(2017, 03, 25, 17, 00, 00),
                    IsActive = true,
                    UserName = "a",
                    CreationDate = DateTime.Now,
                    ActivityId = eventDb.Activities.FirstOrDefault(a => a.ActivityDescr == "Senior's Golf Tournament").ActivityId
                });
                eventDb.Events.Add(new Event()
                {
                    EventDate = new DateTime(2017, 03, 26),
                    EventDateTimeFrom = new DateTime(2017, 03, 26, 15, 00, 00),
                    EventDateTimeTo = new DateTime(2017, 03, 26, 17, 00, 00),
                    IsActive = true,
                    UserName = "a",
                    CreationDate = DateTime.Now,
                    ActivityId = eventDb.Activities.FirstOrDefault(a => a.ActivityDescr == "Senior's Golf Tournament").ActivityId
                });
                eventDb.SaveChanges();
            }
        }
    }    
}
