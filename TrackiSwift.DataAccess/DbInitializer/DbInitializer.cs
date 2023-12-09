using TrackiSwift.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrackiSwift.DataAccess.DbInitializer;
using TrackiSwift.Models;
using TrackiSwift.Utility;

namespace TrackiSwift.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {

            //applying migration auto
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            //create roles if not created



            //if roles are not created, then we will create admin user as well

            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Rider)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Client)).GetAwaiter().GetResult();

                //if roles are not created, then we will create admin user as well
                var result=_userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "shuvam",
                    Email = "shuvamupreti@gmail.com",
                    PhoneNumber = "1234567890",
                    City = "Kathmandu",
                    WardNo = 11,
                    StreetAddress = "Babarmahal",

                }, "Admin@123").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "shuvamupreti@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();


            }

            return;
        }

    }
}
