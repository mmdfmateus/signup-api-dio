using ApiValidation.Models;
using ApiValidation.Services;
using EmailValidation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiValidation.Controllers
{
    [Route("signup")]
    public class SignUpController : ControllerBase
    {
        private readonly IUserService _userService;

        public SignUpController(IUserService userService)
        {
            _userService = userService;
        }

        // POST localhost:5000/signup
        [HttpPost]
        public async Task<IActionResult> CreateUser(SignUpRequest signUpRequest)
        {
            if(signUpRequest.Password != signUpRequest.PasswordConfirmation)
            {
                return BadRequest("O campo 'Password' e 'PasswordConfirmation' devem ser iguais");
            }

            var isEmailValid = EmailValidator.Validate(signUpRequest.Email);
            
            if (!isEmailValid)
            {
                return BadRequest("O email fornecido não é valido");
            }

            var user = await _userService.CreateUser(signUpRequest.Name, signUpRequest.Email, signUpRequest.Password);

            return Ok(user);
        }
    }
}
