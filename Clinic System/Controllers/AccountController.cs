using Clinic_System.Data.Helpers;

namespace Clinic_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        [HttpPost("Register")]
        public async Task <IActionResult> Register(RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.Email = registerDTO.Email;
                applicationUser.UserName = registerDTO.UserName;


                IdentityResult result= await userManager.CreateAsync(applicationUser, registerDTO.Password);

                if (result.Succeeded)
                {
                    return Ok("Create");
                }

                foreach (var i in result.Errors)
                {
                    ModelState.AddModelError("Password", i.Description);
                }
            }
           
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginDTO.UserName);
                if (user != null)
                {
                    var found = await userManager.CheckPasswordAsync(user, loginDTO.Password);

                    if (found)
                    {
                        //Generate Token

                        List<Claim> UserCliams = new List<Claim>();

                        // Token Genarated id Change (JWT Predefind Claims)
                        UserCliams.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        
                        UserCliams.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        UserCliams.Add(new Claim(ClaimTypes.Name, user.UserName));

                        var userRoles = await userManager.GetRolesAsync(user);

                        foreach (var role in userRoles)
                        {
                            UserCliams.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var singinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecritKey"]));
                        var singinCred = new SigningCredentials(singinKey, SecurityAlgorithms.HmacSha256);


                        //Design Token
                        var egyptTime = EgyptTimeHelper.GetEgyptTime();
                        var expirationTime = egyptTime.AddHours(1);

                        JwtSecurityToken MyToken = new JwtSecurityToken(
                            issuer: config["JWT:IssuerIP"],
                            audience: config["JWT:AudienceIP"], // Is For Anglur
                            expires: expirationTime,
                            claims: UserCliams,
                            signingCredentials: singinCred
                            );

                        //Generate Token Response

                        return Ok(new
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                            expiration = expirationTime
                        });
                    }
                }
                ModelState.AddModelError("UserName", "invalid");
            }
            return BadRequest(ModelState);

        }
    }
}
