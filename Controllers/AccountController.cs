using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(
     UserManager<ApplicationUser> userManager,
     RoleManager<IdentityRole<Guid>> roleManager,
     SignInManager<ApplicationUser> signInManager,
     IConfiguration configuration) : Controller
    {

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(Register register)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.UserName = register.UserName;
                applicationUser.Email = register.Email;
                applicationUser.PasswordHash = register.Password;
                IdentityResult identityResult = await userManager.CreateAsync(applicationUser, register.Password);
                if (identityResult.Succeeded)
                {
                    return Ok("Success");
                    //await signInManager.SignInAsync(applicationUser,isPersistent:true);
                }
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LogIn log)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(log.Name);
                if (user != null && await userManager.CheckPasswordAsync(user, log.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        //new Claim(ClaimTypes.Role, user.Role), // Add the role here
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                    };

                    claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                    var signingCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: configuration["JWT:ValidIssuer"],
                        audience: configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(1),
                        claims: claims,
                        signingCredentials: signingCredentials
                    );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = DateTime.Now.AddHours(1)
                    });
                }
                ModelState.AddModelError("", "Wrong Username or Password");
            }
            return BadRequest(ModelState);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name cannot be empty.");
            }

            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return BadRequest("Role already exists.");
            }

            // Use IdentityRole<Guid> instead of IdentityRole
            IdentityRole<Guid> identityRole = new IdentityRole<Guid>
            {
                Name = roleName
            };

            var result = await roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return Ok(new { RoleName = roleName });
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AssignRoleToUser")]
        public async Task<IActionResult> AssignRoleToUser(string userName, string roleName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return BadRequest("Role does not exist.");
            }

            // Use IdentityRole<Guid> instead of IdentityRole
            var result = await userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok("User assigned to role successfully.");
            }

            return BadRequest(result.Errors);
        }

    }
}
