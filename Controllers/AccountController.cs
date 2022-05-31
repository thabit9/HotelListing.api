using AutoMapper;
using HotelListing.api.DTO;
using HotelListing.api.Models;
using HotelListing.api.Sevices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<ApiUser> userManager,
            SignInManager<ApiUser> signInManager,
            ILogger<AccountController> logger, 
            IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _authManager = authManager;
        }

        //POST api/account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration Attempt for {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _userManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                //return StatusCode(500, "Internal Server Error. Please Try Again Later.");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        //POST api/account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation($"Login Attempt for {loginDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                /*var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, false, false);
                if (!result.Succeeded)
                {
                    return Unauthorized(loginDTO);
                }
                return Accepted();*/
                if (!await _authManager.ValidateUser(loginDTO))
                {
                    return Unauthorized();
                }
                return Accepted(new { Token = await _authManager.CreateToken() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Login)}");
                //return StatusCode(500, "Internal Server Error. Please Try Again Later.");
                return Problem($"Something went wrong in the {nameof(Login)}", statusCode: 500);
            }
        }
    }
}