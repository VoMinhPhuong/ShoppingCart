
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Data;
using ShoppingCart.Models;
using ShoppingCart.Utility.DbInitialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Utility.DbInitializer
{
    public class DbInitializerRepo : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ShoppingDbContext _context;

        public DbInitializerRepo(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            ShoppingDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context; 
        }

        public void Initialize()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw new Exception("Err Initialize!");
            }

            if (!_roleManager.RoleExistsAsync(WebSiteRole.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebSiteRole.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRole.Role_User)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRole.Role_Employee)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "addmin@gmail.com",
                    Email = "addmin@gmail.com",
                    Name = "Admin",
                    PhoneNumber = "123456",
                    Address = "abc",
                    City = "abc",
                    State = "abc",
                    PinCode = "1111"
                }, "Admin@123").GetAwaiter().GetResult();

                ApplicationUser user = _context.ApplicationUsers.
                    FirstOrDefault(x => x.Email == "addmin@gmail.com");
                _userManager.AddToRoleAsync(user, WebSiteRole.Role_Admin).GetAwaiter().GetResult();

            }
            return;
        }
    }
}
