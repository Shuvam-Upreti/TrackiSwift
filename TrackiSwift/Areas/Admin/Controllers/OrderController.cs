using MeetingRoom.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using TrackiSwift.Models;
using TrackiSwift.Models.Models;

namespace TrackiSwift.Areas.Client.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AutoValidateAntiforgeryToken]
        //Action
        [HttpGet]
        public IActionResult Index()
        {
           // IEnumerable<Order> objOrderList = _db.Orders.ToList();
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Order obj)
        {
            if (ModelState.IsValid)
            {
                _db.Orders.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Created sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var orderFromDb = _db.Orders.FirstOrDefault(u => u.OrderId == Id);

            if (orderFromDb == null)
            {
                return NotFound();
            }
            return View(orderFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Order obj)
        {
            if (ModelState.IsValid)
            {
                _db.Orders.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Edited Sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //[HttpPost]
        //public IActionResult Edit(Order obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Orders.Update(obj);
        //        _db.SaveChanges();
        //        TempData["success"] = "Edited successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View(obj);
        //}
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _db.Orders.FirstOrDefault(u => u.OrderId == Id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDb);
        }
        [HttpPost]
        public IActionResult Delete(Order obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.DeliveryStatus =="Delivered" || obj.DeliveryStatus == "Returned")
                {
                    OrderBackup backupOrders = new OrderBackup
                    {
                       OrderId = obj.OrderId,
                       ReceiverName = obj.ReceiverName,
                       ReceiverNumber = obj.ReceiverNumber,
                       CreatedDateTime = DateTime.Now,
                       DeliveryAddress = obj.DeliveryAddress,
                       Weight = obj.Weight,
                       Amount = obj.Amount,
                       DeliveryStatus = obj.DeliveryStatus,
                       PaymentStatus = obj.PaymentStatus,
                    };
                    _db.OrderBackups.Add(backupOrders);
                    _db.SaveChanges();
                }

                _db.Orders.Remove(obj);
                _db.SaveChanges();
                TempData["success"] = "Deleted sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Import(IFormFile file)
        {
            if (file == null)
            {
                TempData["error"] = "Please select a file.";
                return RedirectToAction("Index"); ;
            }
            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; 

                        List<Order> orders = new List<Order>();

                        for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                        {
                            Order order = new Order
                            {
                                ReceiverName = worksheet.Cells[row, 1].Value?.ToString(),
                                DeliveryAddress = worksheet.Cells[row, 2].Value?.ToString(),
                                Weight = Convert.ToDouble(worksheet.Cells[row, 3].Value),
                                Amount = Convert.ToDouble(worksheet.Cells[row, 4].Value),

                            };
                            orders.Add(order);
                        }
                        if (orders.Count > 0)
                        {
                            _db.Orders.AddRange(orders);
                            _db.SaveChanges();
                        }

                        TempData["success"] = "Imported successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Error during import: {ex.Message}";
                return RedirectToAction("Index");
            }
        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _db.Orders.ToList();
            return Json(new { data = productList });

        }
        #endregion
    }
}
