using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZenithWebSite.Models.AccountViewModels
{
    public class AddRoleViewModel
    {
        public List<SelectListItem> UserLists { get; set; }
        public List<SelectListItem> ApplicationRoles { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string ApplicationRoleId { get; set; }
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }
    }
}
