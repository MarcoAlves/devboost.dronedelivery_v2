using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using devboost.dronedelivery.felipe.Facade.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace devboost.dronedelivery.felipe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {

        private readonly IClienteRepository _clienteRepository;
        private readonly IClienteFacade _clienteFacade;


        public ClienteController(IClienteRepository clienteRepository, IClienteFacade clienteFacade)
        {
            _clienteRepository = clienteRepository;
            _clienteFacade = clienteFacade;
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            await _clienteFacade.Save(cliente);


            return CreatedAtRoute("Login", cliente , "");
                
        }

        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> GetAll()
        {
            var clientes = _clienteRepository.GetAll();

            return Ok(clientes);
        }

    }
}
