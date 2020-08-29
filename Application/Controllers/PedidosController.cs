using devboost.dronedelivery.felipe.DTO.Enums;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using devboost.dronedelivery.felipe.Facade.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.Controllers
{
    /// <summary>
    /// Controller com as operações dos pedidos
    /// </summary>
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoFacade _pedidoFacade;
        private readonly IPedidoRepository _pedidoRepository;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public PedidosController(IPedidoRepository pedidoRepository, IPedidoFacade pedidoFacade)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _pedidoFacade = pedidoFacade;
            _pedidoRepository = pedidoRepository;
        }

        /// <summary>
        /// Percorre lista de pedidos em espera adicionando um drone para os mesmos
        /// </summary>
        /// <returns></returns>
        [HttpPost("assign-drone")]
        public async Task<ActionResult> AssignDrone()
        {
            await _pedidoFacade.AssignDrone();
            return Ok();
        }


        /// <summary>
        /// Adiciona um novo pedido
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            pedido.DataHoraInclusao = DateTime.Now;
            pedido.Situacao = (int)StatusPedido.AGUARDANDO;
            await _pedidoRepository.SavePedidoAsync(pedido);

            return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
        }


    }
}
