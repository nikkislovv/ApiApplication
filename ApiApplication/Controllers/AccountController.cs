using ApiApplication.Auth;
using ApiApplication.Contracts;
using ApiApplication.Models;
using AutoMapper;
using Contracts;
using Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class AccountController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly UserManager<User> _userManager;
        readonly IAuthenticationManager _authManager;
        readonly ILoggerManager _logger;


        public AccountController(UserManager<User> userManager, IMapper mapper, IAuthenticationManager authManager , ILoggerManager logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _authManager = authManager;
            _logger = logger;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="userForRegistrationDto"></param>
        /// <returns></returns>
        /// <response code="201">User created</response>
        /// <response code="400">New user is null</response>
        /// <response code="422">New user is invalid</response>
        /// <response code="500">Server error</response>
        [HttpPost("SignUp")]//регистрация
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            var user = _mapper.Map<User>(userForRegistrationDto);
            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRolesAsync(user, userForRegistrationDto.Roles);
            return StatusCode(201);
        }

        /// <summary>
        /// Get JWT token for auth
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>JWT token</returns>
        /// <response code="200">Returns JWT token</response>
        /// <response code="400">User is null</response>
        /// <response code="401">Anuthorized user</response>
        /// <response code="422">User is invalid</response>
        /// <response code="500">Server error</response>
        [HttpPost("SignIn")]//аутентификация
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userDto)
        {
            if (!await _authManager.ValidateUser(userDto))
            {
                _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }
            var result = new
            {
                UserName = userDto.UserName,
                Token = await _authManager.CreateToken()
            };
            return Ok(result);
        }


    }
}
