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
using Microsoft.AspNetCore.Http;

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
                        join ur in _context.UserRoles on u.Id equals ur.UserId
                        join r in _context.Roles on ur.RoleId equals r.Id
                        select new { u.FirstName, u.LastName, u.UserName, u.Email, r.Name, ur.RoleId, ur.UserId };
            
            foreach (var ulist in users)
            {
                model.Add(new UserListViewModel
                {
                    UserRoleId = (ulist.UserName + "&"+ ulist.Name),
                    FirstName = ulist.FirstName,
                    LastName = ulist.LastName,
                    UserName = ulist.UserName,
                    Email = ulist.Email,
                    RoleName = ulist.Name
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
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Index");
                        }
                    }
                    
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAssignedRole(string id)
        {
            string name = string.Empty;
            ApplicationRole ar = new ApplicationRole();
            var model = from ur in _context.UserRoles
                        select ur;
            if (!String.IsNullOrEmpty(id))
            {
                foreach (var a in model)
                {
                    if ((a.UserId + "&" + a.RoleId).Equals(id))
                    {
                        ar = await _roleManager.FindByIdAsync(a.RoleId);
                        name = ar.Name;
                    }
                }
            }
            return PartialView("_DeleteRole", name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAssignedRole(string id, FormCollection form)
        {
            ApplicationRole ar = new ApplicationRole();
            ApplicationUser au = new ApplicationUser();
            var model = from u in _context.Users
                        join ur in _context.UserRoles on u.Id equals ur.UserId
                        join r in _context.Roles on ur.RoleId equals r.Id
                        select new { u.UserName, r.Name, ur.RoleId, ur.UserId };

            if (!String.IsNullOrEmpty(id))
            {
                foreach (var a in model)
                {
                    if ((a.UserName + "&" + a.Name).Equals(id))
                    {
                        ar = await _roleManager.FindByIdAsync(a.RoleId);
                        au = await _userManager.FindByIdAsync(a.UserId);
                    }
                }
                if (ar != null)
                {
                    IdentityResult roleRuslt = await _userManager.RemoveFromRoleAsync(au, ar.Name);
                    if (roleRuslt.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return PartialView("_DeleteRole");
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