using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrackiSwift.Data;

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

        #region API CALLS

        public IActionResult GetAll()
        {
            var user=_db.ApplicationUsers.ToList();
            return Json(new { data=user });
        }


        #endregion
    }
}
