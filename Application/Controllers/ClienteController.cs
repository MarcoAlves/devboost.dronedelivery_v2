using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using devboost.dronedelivery.felipe.Facade.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace devboost.dronedelivery.felipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        private readonly IClienteRepository _clienteRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClienteFacade _clienteFacade;



        public ClienteController(IClienteRepository clienteRepository, IClienteFacade clienteFacade, UserManager<ApplicationUser> userManager)
        {
            _clienteRepository = clienteRepository;
            _clienteFacade = clienteFacade;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {

            await _clienteFacade.Save(cliente);

            var user = new ApplicationUser()
            {
                UserName = cliente.Nome,
                Email = "",
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "AdminAPIDrone01!");

            if (!result.Succeeded)
            {
                await _clienteFacade.Delete(cliente.Id);
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, Roles.ROLE_API_DRONE);

            return Ok();
                
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> GetAll()
        {
            var clientes = _clienteRepository.GetAll();

            return Ok(clientes);
        }

    }
}
