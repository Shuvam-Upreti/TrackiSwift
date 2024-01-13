using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrackiSwift.Data;
using TrackiSwift.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using TrackiSwift.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace TrackiSwift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class RiderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RiderController(ApplicationDbContext db, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;

        }
        public async Task<IActionResult> AssignRider(int id)
        {
            var allUsers = await _db.Users.ToListAsync();

            var riderUserIds = new List<string>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Rider"))
                {
                    riderUserIds.Add(user.Id);
                }
            }

            var riderList = allUsers
                .Where(u => riderUserIds.Contains(u.Id))
                .Select(u => new SelectListItem
                {
                    Text = u.UserName,
                    Value = u.Id
                })
                .ToList();

            ViewBag.UserList = riderList;


            var order = _unitOfWork.Orders.GetFirstOrDefault(u => u.OrderId == id);
            Rider rider = new Rider
            {
                OrderId = id,

            };

            return View(rider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRider(Rider obj)
        {

            var order = _unitOfWork.Orders.GetFirstOrDefault(u => u.OrderId == obj.OrderId);
            var existingRider = _db.Riders.FirstOrDefault(p => p.UserId == obj.UserId && p.OrderId == obj.OrderId);
            if (existingRider == null)
            {

                Rider rider = new Rider
                {
                    UserId = obj.UserId,
                    OrderId = order.OrderId
                };
                _db.Riders.Add(rider);

                // _unitOfWork.Rider.Add(participant);
                TempData["success"] = "Rider Assigned Sucessfully";
                _unitOfWork.Save();
            }
            else
            {
                TempData["error"] = "Rider Already Assigned";
                return View(existingRider);
            }
            return RedirectToAction("Index", "Order", new { area = "Admin" });


        }


        public async Task<IActionResult> ViewRider(Rider obj)
        {
            var existingRider = _db.Riders.FirstOrDefault(p => p.UserId == obj.UserId && p.OrderId == obj.OrderId);
 

            var riders = _db.Riders.Select(x => new SelectListItem
            {
                Value = x.Users.Id,
                Text = x.Users.Name
            }).ToList();
            ViewBag.Roles = riders;

            return View(existingRider);
        }
    }
}
