using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZenithWebSite.Data;
using Microsoft.AspNetCore.Identity;
using ZenithWebSite.Models;
using ZenithWebSite.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace ZenithWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AssignRoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private ApplicationDbContext _context;

        public AssignRoleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<UserListViewModel> model = new List<UserListViewModel>();
            List<string> Roles = new List<string>();

            var users = from u in _context.Users
                        join ru in _context.UserRoles on u.Id equals ru.UserId
                        join r in _context.Roles on ru.RoleId equals r.Id
                        group r by new { u.Id, u.FirstName, u.LastName, u.UserName, u.Email } into g
                        select g;

            foreach (var ulist in users)
            {
                List<string> roles = new List<string>();
                foreach (var u in ulist)
                {
                    roles.Add(u.Name);
                }
                model.Add(new UserListViewModel
                {
                    Id = ulist.Key.Id,
                    FirstName = ulist.Key.FirstName,
                    LastName = ulist.Key.LastName,
                    UserName = ulist.Key.UserName,
                    Email = ulist.Key.Email,
                    RoleNames = roles
                });

            }

            return View(model);
        }
        [HttpGet]
        public IActionResult AssignUser()
        {
            AddRoleViewModel model = new AddRoleViewModel();
            model.UserLists = _userManager.Users.Select(u => new SelectListItem
            {
                Text = u.UserName,
                Value = u.Id
            }).ToList();

            model.ApplicationRoles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            return PartialView("_AssignUser", model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignUser(AddRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                //ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                ApplicationUser user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    ApplicationRole role = await _roleManager.FindByIdAsync(model.ApplicationRoleId);

                    if (!await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, role.Name);
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    
                }
            }

            return RedirectToAction("Index");
        }

        #region Helpers

        private async Task<ApplicationUser> getUser(AddRoleViewModel model)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(model.UserId);
            return user;
        }
        #endregion
    }
}