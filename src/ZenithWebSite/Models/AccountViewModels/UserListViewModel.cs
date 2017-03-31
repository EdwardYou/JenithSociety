using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZenithWebSite.Models.AccountViewModels
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string UserRoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
