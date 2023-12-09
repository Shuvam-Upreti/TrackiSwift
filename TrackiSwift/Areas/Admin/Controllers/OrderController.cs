using TrackiSwift.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using System.Diagnostics;
using System.Security.Claims;
using TrackiSwift.Models;
using TrackiSwift.Models.Models;
using TrackiSwift.Models.ViewModels;
using TrackiSwift.Utility;
using TrackiSwift.DataAccess.Repository.IRepository;

namespace TrackiSwift.Areas.Client.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public OrderController(ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var viewModel = new OrderVM
            {
                ApplicationUserId = userId
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Create(OrderVM obj)
        {
            if (ModelState.IsValid)
            {
                Order objs = new Order()
                {
                    OrderId = obj.OrderId,
                    ApplicationUserId = obj.ApplicationUserId,
                    ReceiverName = obj.ReceiverName,
                    ReceiverNumber = obj.ReceiverNumber,
                    CreatedDateTime = obj.CreatedDateTime,
                    DeliveryAddress = obj.DeliveryAddress,
                    Weight = obj.Weight,
                    Amount = obj.Amount,
                    DeliveryStatus = obj.DeliveryStatus,
                    PaymentStatus = obj.PaymentStatus
                };
                _unitOfWork.Orders.Add(objs);
                _unitOfWork.Save();
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
            var orderFromDb = _unitOfWork.Orders.GetFirstOrDefault(u => u.OrderId == Id);

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
                _unitOfWork.Orders.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Edited Sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _unitOfWork.Orders.GetFirstOrDefault(u => u.OrderId == Id);

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
                if (obj.DeliveryStatus == "Delivered" || obj.DeliveryStatus == "Returned")
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

                _unitOfWork.Orders.Remove(obj);
                _unitOfWork.Save();
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
                                ReceiverNumber = worksheet.Cells[row, 2].Value?.ToString(),
                                DeliveryAddress = worksheet.Cells[row, 3].Value?.ToString(),
                                Weight = Convert.ToDouble(worksheet.Cells[row, 4].Value),
                                Amount = Convert.ToDouble(worksheet.Cells[row, 5].Value),
                                DeliveryStatus = worksheet.Cells[row, 6].Value?.ToString(),
                                PaymentStatus = worksheet.Cells[row, 7].Value?.ToString(),
                                ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)

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
        public IActionResult GetAll(string status)
        {
            IEnumerable<Order> orderList;
             orderList = _unitOfWork.Orders.GetAll();

            switch (status)
            {
                case "unverified":
                    orderList = orderList.Where(u => u.DeliveryStatus == SD.StatusUnverified);
                    break;
                case "inprocess":
                    orderList = orderList.Where(u => u.DeliveryStatus == SD.StatusInProcess);
                    break;
                case "delivered":
                    orderList = orderList.Where(u => u.DeliveryStatus == SD.StatusDelivered);
                    break;
                case "returned":
                    orderList = orderList.Where(u => u.DeliveryStatus == SD.StatusCancelled);
                    break;
                default:
                    break;
            }

            return Json(new { data = orderList });

        }
        #endregion
    }
}
