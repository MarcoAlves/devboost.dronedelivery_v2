using devboost.dronedelivery.felipe.DTO;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using devboost.dronedelivery.felipe.Facade.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Controllers
{
    /// <summary>
    /// Controller com operações referentes aos drones
    /// </summary>
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DronesController : ControllerBase
    {
        private readonly IDroneFacade _droneFacade;
        private readonly IDroneRepository _droneRepository;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public DronesController(IDroneRepository droneRepository, IDroneFacade droneFacade)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _droneFacade = droneFacade;
            _droneRepository = droneRepository;
        }
        /// <summary>
        /// Retorna status dos drones
        /// </summary>
        /// <returns></returns>
        // GET: api/Drones/5
        [HttpGet("GetStatusDrone")]
        public async Task<ActionResult<List<StatusDroneDto>>> GetStatusDrone()
        {
            return Ok(await _droneFacade.GetDroneStatusAsync());
        }

        /// <summary>
        /// Cria um novo drone 
        /// </summary>
        /// <param name="drone">Drone</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles.ROLE_API_DRONE)]
        public async Task<ActionResult<Drone>> PostDrone(Drone drone)
        {
            drone.Perfomance = (drone.Autonomia / 60.0f) * drone.Velocidade;

            await _droneRepository.SaveDrone(drone);

            return CreatedAtAction("GetDrone", new { id = drone.Id }, drone);
        }


    }
}
