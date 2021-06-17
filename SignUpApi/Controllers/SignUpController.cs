using ApiValidation.Models;
using ApiValidation.Services;
using Microsoft.AspNetCore.Mvc;
using SignUpApi.Validators;
using System;
using System.Threading.Tasks;

namespace ApiValidation.Controllers
{
    [Route("signup")]
    public class SignUpController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IEmailValidator _emailValidator;

        public SignUpController(IUserService userService, IEmailValidator emailValidator)
        {
            _userService = userService;
            _emailValidator = emailValidator;
        }

        // POST localhost:5000/signup
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(SignUpRequest signUpRequest)
        {
            try
            {
                if (signUpRequest.Password != signUpRequest.PasswordConfirmation)
                {
                    return BadRequest("O campo 'Password' e 'PasswordConfirmation' devem ser iguais");
                }

                var isEmailValid = _emailValidator.Validate(signUpRequest.Email);

                if (!isEmailValid)
                {
                    return BadRequest("O email fornecido não é valido");
                }

                var user = await _userService.CreateUser(signUpRequest.Name, signUpRequest.Email, signUpRequest.Password);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
