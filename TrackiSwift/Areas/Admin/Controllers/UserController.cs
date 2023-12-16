using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrackiSwift.Data;
using TrackiSwift.Models;
using TrackiSwift.Models.ViewModels;
using TrackiSwift.Utility;

namespace TrackiSwift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UserController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        private RoleManager<IdentityRole> _roleManager;
        public UserController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            var userFromDb = _db.Users.Find(id);

            if (userFromDb == null)
            {
                return NotFound();
            }
            var roles = _roleManager.Roles.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Name
            }).ToList();
            ViewBag.Roles = roles;
            return View(userFromDb);
        }
        [HttpPost]
        public IActionResult Edit(UserVM user)
        {
            if (ModelState.IsValid)
            {

                ApplicationUser users = new ApplicationUser() 
                { 
                    Name=user.Name,
                    Email=user.Email,
                    Phone=user.Phone,
                    City=user.City,
                    StreetAddress=user.StreetAddress,
                    WardNo=user.WardNo,
                    Role=user.Role,
                };

                _db.Users.Update(users);
                _db.SaveChanges();
                TempData["success"] = "Edited Sucessfully";
                return RedirectToAction("Index");
            }
            return View(user);
        }


        #region API CALLS

        public IActionResult GetAll(string status)
        {
            var userList = _db.ApplicationUsers.ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;


            }
            return Json(new { data = userList });
        }
        #endregion
    }
}
