using devboost.dronedelivery.felipe.DTO.Constants;
using devboost.dronedelivery.felipe.EF.Data;
using Microsoft.Extensions.Configuration;

namespace devboost.dronedelivery.felipe.EF.Repositories
{
    public abstract class RepositoryBase
    {
        protected readonly DataContext _context;
        protected readonly string _connectionString;

        protected RepositoryBase(DataContext context,
            IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString(ProjectConsts.CONNECTION_STRING_CONFIG);

        }


    }
}
