using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace devboost.dronedelivery.felipe.Controllers
{

    /// <summary>
    /// Controller com operações referentes aos drones
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController
    {
        public LoginController()
        {
        }

        [AllowAnonymous]
        [HttpPost]
        public object Post([FromBody] User usuario, [FromServices] AccessManager accessManager)
        {
            if (accessManager.ValidateCredentials(usuario))
            {
                return accessManager.GenerateToken(usuario);
            }
            else
            {
                return new
                {
                    Authenticated = false,
                    Message = "Falha ao autenticar"
                };
            }
        }


    }
}
