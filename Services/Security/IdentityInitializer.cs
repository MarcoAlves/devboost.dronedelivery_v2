using devboost.dronedelivery.felipe.DTO.Models;
using devboost.dronedelivery.felipe.EF.Data;
using Microsoft.AspNetCore.Identity;
using System;

namespace devboost.dronedelivery.felipe.Security
{
    public class IdentityInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityInitializer(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_context.Database.EnsureCreated())
            {
                if (!_roleManager.RoleExistsAsync(Roles.ROLE_API_DRONE).Result)
                {
                    var resultado = _roleManager.CreateAsync(
                        new IdentityRole(Roles.ROLE_API_DRONE)).Result;
                    if (!resultado.Succeeded)
                    {
                        throw new Exception(
                            $"Erro durante a criação da role {Roles.ROLE_API_DRONE}.");
                    }
                }

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "admin_drone",
                        Email = "admin-apiprodutos@teste.com.br",
                        EmailConfirmed = true
                    }, "AdminAPIDrone01!", Roles.ROLE_API_DRONE);

                CreateUser(
                    new ApplicationUser()
                    {
                        UserName = "usuario_drone",
                        Email = "usrinvalido-apiprodutos@teste.com.br",
                        EmailConfirmed = true
                    }, "UsrInvAPIDrone01!");
            }
        }
        private void CreateUser(
            ApplicationUser user,
            string password,
            string initialRole = null)
        {
            if (_userManager.FindByNameAsync(user.UserName).Result == null)
            {
                var resultado = _userManager
                    .CreateAsync(user, password).Result;

                if (resultado.Succeeded &&
                    !String.IsNullOrWhiteSpace(initialRole))
                {
                    _userManager.AddToRoleAsync(user, initialRole).Wait();
                }
            }
        }
    }
}
