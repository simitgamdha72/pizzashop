using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;

namespace pizzashop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly PizzashopContext _context;

        public OrdersController(PizzashopContext context)
        {
            _context = context;
        }

        public IActionResult Orders()
        {
            return View();
        }


        public IActionResult ShowOrders([FromQuery] OrderSearchViewModel searchModel)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.SearchId))
            {
                query = query.Where(o => o.Id.ToString().Contains(searchModel.SearchId));
            }

            if (!string.IsNullOrEmpty(searchModel.Status) && searchModel.Status != "")
            {
                query = query.Where(o => o.Status.ToString() == searchModel.Status);
            }

            if (searchModel.StartDate.HasValue)
            {
                query = query.Where(o => o.Date >= searchModel.StartDate.Value);
            }

            if (searchModel.EndDate.HasValue)
            {
                query = query.Where(o => o.Date <= searchModel.EndDate.Value);
            }

            switch (searchModel.DateRange)
            {
                case "LAST 7 DAYS":
                    query = query.Where(o => o.Date >= DateTime.Now.AddDays(-7));
                    break;
                case "LAST 30 DAYS":
                    query = query.Where(o => o.Date >= DateTime.Now.AddDays(-30));
                    break;
                case "CURRENT MONTH":
                    var now = DateTime.Now;
                    query = query.Where(o => o.Date.Month == now.Month && o.Date.Year == now.Year);
                    break;
            }

            // Sorting
            query = searchModel.SortColumn switch
            {
                "id" => searchModel.SortDirection == "asc"
                    ? query.OrderBy(o => o.Id)
                    : query.OrderByDescending(o => o.Id),
                "customername" => searchModel.SortDirection == "asc"
                    ? query.OrderBy(o => o.Customer.Name)
                    : query.OrderByDescending(o => o.Customer.Name),
                "orderdate" => searchModel.SortDirection == "asc"
                    ? query.OrderBy(o => o.Date)
                    : query.OrderByDescending(o => o.Date),
                _ => query.OrderByDescending(o => o.Date)
            };

            int totalItems = query.Count();

            int pageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 5;
            int currentPage = searchModel.CurrentPage > 0 ? searchModel.CurrentPage : 1;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedOrders = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    OrderDate = o.Date,
                    OrderType = o.OrderType,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name,
                    Status = o.Status,
                    PaymentMode = o.Payments.Any() ? o.Payments.First().PaymentMode : "pending",
                    Rating = o.Ratings.Any() ? o.Ratings.First().Rating1 : 0,
                    TotalAmount = o.Payments.Any() ? o.Payments.First().Amount : 0,
                    Instruction = o.Comment,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                }).ToList();

            ViewBag.totalitemscount = totalItems;
            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = currentPage;

            return PartialView("_OrderTable", pagedOrders);
        }




        public async Task<MemoryStream> ExportDataToExcelAsync()
        {
            var data = await _context.Orders.ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");


                worksheet.Cells[1, 1].Value = "Column1";
                worksheet.Cells[1, 2].Value = "Column2";



                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].Id;
                    worksheet.Cells[i + 2, 2].Value = data[i].Date;
                    worksheet.Cells[i + 2, 3].Value = data[i].Customer;
                    worksheet.Cells[i + 2, 4].Value = data[i].Status;
                    worksheet.Cells[i + 2, 5].Value = data[i].Payments;
                    worksheet.Cells[i + 2, 6].Value = data[i].Ratings;
                    // worksheet.Cells[i + 2, 7].Value = data[i].Payment;

                }

                var memoryStream = new MemoryStream();
                await package.SaveAsAsync(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var excelFile = await ExportDataToExcelAsync();
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportedData.xlsx");
        }






    }
}