using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace ZenithWebSite.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
