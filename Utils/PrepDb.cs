using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyPrimerWebApi.Data;
using MyPrimerWebApi.Models;
using Polly;

namespace MyPrimerWebApi.Utils
{
    public class PrepDb : IPrepDb
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PrepDb(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initialize()
        {
            try
            {
                var retry = Policy.Handle<SqlException>()
                      .WaitAndRetry(new TimeSpan[]
                      {
                        TimeSpan.FromSeconds(2),
                        TimeSpan.FromSeconds(6),
                        TimeSpan.FromSeconds(12)
                      });

                retry.Execute(() =>
                    _context.Database.Migrate());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (_context.Roles.AnyAsync(x => x.Name == "Admin").GetAwaiter().GetResult())
            {
                return;
            }
            _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
            }, "Admin123*").GetAwaiter().GetResult();

            _userManager.AddToRoleAsync(_context.Users.FirstOrDefaultAsync(x => x.Email == "admin@gmail.com").GetAwaiter().GetResult(), "Admin").GetAwaiter().GetResult();
        }
    }
}