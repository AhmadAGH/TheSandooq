using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TheSandooq.Models;
using Sanooqna.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using TheSandooq.Data;

namespace Sanooqna.Controllers
{
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private string _currentUserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        public MembersController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        // GET: Members
        public async Task<ActionResult> MemberDetailsAsync(string memberID, int sandooqID)
        {
            ApplicationUser member = await _userManager.FindByIdAsync(memberID);
            Sandooq sandooq = await _dbContext.dbSandooqs.FindAsync(sandooqID);
            return View();
        }
    }
}