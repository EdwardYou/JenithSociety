using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZenithWebSite.Models.AccountViewModels;
using ZenithWebSite.Models;
using ZenithWebSite.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ZenithWebSite.Controllers
{
    [Produces("application/json")]
    [Route("api/Registration")]
    public class RegisterAPIController : Controller
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public RegisterAPIController(ApplicationDbContext context,
                             UserManager<ApplicationUser> userManager,
                             SignInManager<ApplicationUser> signInManager,
                             ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
        // POST: api/Registration
        [HttpPost]
        public async void Post([FromBody]RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.UserName, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                await _userManager.AddToRoleAsync(user, "Member");
                _logger.LogInformation(3, "User created a new account with password.");
            }
        }
    }
}
