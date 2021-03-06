using devboost.dronedelivery.felipe.DTO.Constants;
using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Data;
using devboost.dronedelivery.felipe.EF.Repositories;
using devboost.dronedelivery.felipe.EF.Repositories.Interfaces;
using devboost.dronedelivery.felipe.Facade;
using devboost.dronedelivery.felipe.Facade.Interface;
using devboost.dronedelivery.felipe.Security;
using devboost.dronedelivery.felipe.Security.Extensions;
using devboost.dronedelivery.felipe.Services;
using devboost.dronedelivery.felipe.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace devboost.dronedelivery.felipe
{
    public class Startup
    {
        private const string SWAGGERFILE_PATH = "./swagger/v1/swagger.json";
        private const string API_VERSION = "v1";
        private const string LOCALHOST = "http://localhost:80";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDroneRepository, DroneRepository>();
            services.AddSingleton<IPedidoDroneRepository, PedidoDroneRepository>();
            services.AddSingleton<IClienteRepository, ClienteRepository>();
            services.AddSingleton<IPedidoRepository, PedidoRepository>();

            services.AddSingleton<IPedidoService, PedidoService>();
            services.AddSingleton<IDroneService, DroneService>();
            services.AddSingleton<ICoordinateService, CoordinateService>();

            services.AddSingleton<IPedidoFacade, PedidoFacade>();
            services.AddSingleton<IDroneFacade, DroneFacade>();
            services.AddSingleton<IClienteFacade, ClienteFacade>();

            // Configurando o uso da classe de contexto para
            // acesso �s tabelas do ASP.NET Identity Core

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseInMemoryDatabase("InMemoryDatabase"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(ProjectConsts.CONNECTION_STRING_CONFIG)));

            // Ativando a utiliza��o do ASP.NET Identity, a fim de
            // permitir a recupera��o de seus objetos via inje��o de
            // depend�ncias
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configurando a depend�ncia para a classe de valida��o
            // de credenciais e gera��o de tokens
            services.AddScoped<AccessManager>();

            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            // Aciona a extens�o que ir� configurar o uso de
            // autentica��o e autoriza��o via tokens
            services.AddJwtSecurity(
                signingConfigurations, tokenConfigurations);

            services.AddAuthorization();

            services.AddCors();
            services.AddControllers();

            services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString(ProjectConsts.CONNECTION_STRING_CONFIG)), ServiceLifetime.Singleton);
            AddSwagger(services);
        }

        private void AddSwagger(IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(API_VERSION, new OpenApiInfo { Title = ProjectConsts.PROJECT_NAME, Version = API_VERSION });
                var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ProjectConsts.XML_EXTENSION;
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Cria��o de estruturas, usu�rios e permiss�es
            // na base do ASP.NET Identity Core (caso ainda n�o
            // existam)
            new IdentityInitializer(context, userManager, roleManager).Initialize();

            // Swagger
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.RoutePrefix = string.Empty;
                   c.SwaggerEndpoint(SWAGGERFILE_PATH, ProjectConsts.PROJECT_NAME + API_VERSION);
               });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
