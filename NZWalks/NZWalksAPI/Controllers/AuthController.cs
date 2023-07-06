using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            //validate the incoming request
            //Hecho con fluent validation


            //ver si el usuario esta autenticado
            //revisar user y pass
            var user = await userRepository.AuthenticateAsync(loginRequest.UserName, loginRequest.Password);

            if (user!=null)
            {
                //Generamos un JWT Token
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);

            }
            return BadRequest("User or Pass incorrect");

        }
    }
}
