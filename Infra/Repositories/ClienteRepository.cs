using devboost.dronedelivery.felipe.DTO.Constants;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace devboost.dronedelivery.felipe.EF.Repositories
{
    public class ClienteRepository : IClienteRepository
    {

        private readonly DataContext _context;
        private readonly string _connectionString;

        public ClienteRepository(DataContext context,
            IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(ProjectConsts.CONNECTION_STRING_CONFIG);

        }

        public Task<Cliente> GetById(int clienteId)
        {
            var cliente = _context.Cliente.FirstOrDefaultAsync(_ => _.Id == clienteId);

            return cliente;
        }

        public async Task<List<Cliente>> GetAll()
        {
            var clientes = _context.Cliente.ToList();
            
            return clientes;
        }

        public Task<Cliente> GetByName(string nome)
        {
            var cliente  = _context.Cliente.FirstOrDefaultAsync(_ => _.Nome == nome);

            return cliente;

        }

        public async Task Save(Cliente cliente)
        {
            _context.Cliente.Add(cliente);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int clienteId)
        {
            var cliente = _context.Cliente.FirstOrDefault(_ => _.Id == clienteId);

            _context.Cliente.Remove(cliente);

            await _context.SaveChangesAsync();
        }
    }
}
