using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrackiSwift.Data;
using TrackiSwift.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using TrackiSwift.Models.Models;
using Microsoft.AspNetCore.Identity;
using TrackiSwift.Models;
using TrackiSwift.Models.ViewModels;

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

            IEnumerable<SelectListItem> riders = _db.Riders.Select(
               x => new SelectListItem
               {
                   Value = x.Users.Id,
                   Text = x.Users.Name
               });
            ViewBag.Roles = riders;

            /////ya bata 
           
            return View(rider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRider(Rider obj)
        {

            var order = _unitOfWork.Orders.GetFirstOrDefault(u => u.OrderId == obj.OrderId);
            var existingRider = _db.Riders.FirstOrDefault(p => p.UserId == obj.UserId && p.OrderId == obj.OrderId);
            var orderExists = _db.Riders.Any(r => r.OrderId == obj.OrderId);
            if (existingRider == null && !orderExists)
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
            }
            return RedirectToAction("Index", "Order", new { area = "Admin" });
        }


        public async Task<IActionResult> ViewRider(int id)
        {
            List<Rider> participants = _db.Riders.ToList();
            List<Rider> participantslist = new List<Rider>();

            foreach (var item in participants)
            {
                var user = _db.Users.Find(item.UserId);
                ApplicationUser abc = new ApplicationUser
                {
                    Id = user.Id,
                    UserName=user.UserName,
                };

                if (item.OrderId == id)
                {
                    item.Users = abc;
                    participantslist.Add(item);
                }
            }

            RiderVM model= new RiderVM
            {
                Riders = participantslist,
            };

            return View(model);
        }
    }
}
