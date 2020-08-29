using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.Facade.Interface;
using devboost.dronedelivery.felipe.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Controllers
{

    /// <summary>
    /// Controller com operações referentes aos drones
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClienteFacade _clienteFacade;

        public LoginController(UserManager<ApplicationUser> userManager, IClienteFacade clienteFacade)
        {
            _userManager = userManager;
            _clienteFacade = clienteFacade;
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

        [AllowAnonymous]
        [HttpPost]
        [Route("CreateUser")]
        public ActionResult CreateUser(User usuario)
        {


            var user = _userManager.CreateAsync(
                new ApplicationUser()
                    {
                        UserName = usuario.UserID,
                        Email = "",
                        EmailConfirmed = true
                    }, "AdminAPIDrone01!");

            var cliente = _clienteFacade.Save(new Cliente()
            {
                Latitude = usuario.Latitude,
                Longitude = usuario.Longitude,
                Nome = usuario.UserID,
                UserId = user.Id
            });
                
            return Ok();

        }


    }
}
