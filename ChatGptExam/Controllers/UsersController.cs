using ChatGptExam.Models;
using ChatGptExam.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatGptExam.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.Admin}")]
        [Route("GetUsers")]
        public IActionResult Get()
        {
            var users = _userManager.Users.ToList();
            if(users == null||users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet]
        [Route("GetUserId")]
        public IActionResult GetUserId()
        {
            var userId = User.Identity.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            return Ok(userId);
        }


        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin}")]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return NotFound($"User with username {UserName} not found");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok($"User with username {UserName} deleted successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting user with username {UserName}");
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin}")]
        [Route("BlockUser")]
        public async Task<IActionResult> BlockUser([FromQuery] string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return NotFound($"User with username {UserName} not found");
            }

            var result = await _userManager.SetLockoutEnabledAsync(user, true);

            if (result.Succeeded)
            {
                return Ok($"User with username {UserName} blocked successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while blocking user with username {UserName}");
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin}")]
        [Route("UnblockUser")]
        public async Task<IActionResult> UnblockUser([FromQuery] string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                return NotFound($"User with username {UserName} not found");
            }

            var result = await _userManager.SetLockoutEnabledAsync(user, false);

            if (result.Succeeded)
            {
                return Ok($"User with username {UserName} unblocked successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while unblocking user with username {UserName}");
            }
        }

    }
}
