using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public UserController(ApplicationDbContext db)
        {
            _db = db;
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
                    WardNo=user.WardNo
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
