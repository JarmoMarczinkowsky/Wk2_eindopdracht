using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EindOpdracht.WebApi.Controllers
{
    [Route("account/[controller]")]
    [Authorize]
    public class AccountManagementController : Controller
    {
        private readonly ILogger<AccountManagementController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountManagementController(ILogger<AccountManagementController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        //[HttpPost("{id}/{claim}/{value?}", Name = "AddClaimToUser")]
        //public async Task<ActionResult<AccountViewModel>> AddAdminClaimToUser(string id, string claim, string value = null!)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }

        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null) {
        //        return NotFound();

        //    }

        //    var addClaimsResult = await _userManager.AddClaimAsync(user, new Claim(claim, value));
        //    if (!addClaimsResult.Succeeded)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok(new);
        //}
    }
}
