using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrackiSwift.Data;
using TrackiSwift.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> AssignRider(int Id)
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


            var order= _unitOfWork.Orders.GetFirstOrDefault(u=>u.OrderId == Id);


            return View(order);
        }
    }
}
