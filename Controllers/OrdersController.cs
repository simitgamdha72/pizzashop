using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;
using pizzashop.Repository;

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
        // public IActionResult OrderDetails()
        // {
        //     return View();
        // }


        public IActionResult ShowOrders([FromQuery] OrderSearchViewModel searchModel)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.SearchId))
            {
                query = query.Where(o => o.Id.ToString().Contains(searchModel.SearchId) || o.Customer.Name.ToLower().Contains(searchModel.SearchId.ToLower()));
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


        // public IActionResult OrderDetails(int id)
        // {
        //     var order = _context.Orders.FirstOrDefault(o => o.Id == id);
        //     var tableorder = _context.Tableorders.FirstOrDefault(t => t.OrderId == id);
        //     var table = _context.Tables.FirstOrDefault(t => t.Id == tableorder.TableId);
        //     var Section = _context.Sections.FirstOrDefault(s => s.Id == table.SectionId);
        //     var customer = _context.Customers.FirstOrDefault(c => c.Id == order.CustomerId);
        //     var invoice = _context.Invoices.FirstOrDefault(i => i.OrderId == order.Id);


        //     var orderitemList = (from o in _context.Ordersitemdetails
        //                          join mi in _context.MenuItems on o.ItemsId equals mi.Id
        //                          join mod in _context.ModifiedOrderDetails on o.OrderId equals mod.OrdersItemDetailsId
        //                          where o.OrderId == order.Id
        //                          select new
        //                          {
        //                              o,
        //                              mi,
        //                              mod
        //                          }).ToList();

        //     if (order == null)
        //     {
        //         return RedirectToAction("Orders", "Orders");
        //     }
        //     var ordersitemdetails = new List<Ordersitemdetail>();
        //     var MenuItems = new List<Tuple<MenuItem, ModifiedOrderDetail>>();

        //     foreach (var item in orderitemList)
        //     {
        //         ordersitemdetails.Add(item.o);
        //         MenuItems.Add(new Tuple<MenuItem, ModifiedOrderDetail>(item.mi, item.mod));
        //     }
        //     var modifiedorderList = _context.ModifiedOrderDetails.AsEnumerable().Where(m => ordersitemdetails.Any(i => i.Id == m.Id)).ToList();

        //     var viewModel = new OrderDetailsViewModel
        //     {

        //         OrderDate = order.Date,
        //         OrderModifiedDate = order.ModifiedAt,
        //         OrderStatus = order.Status,
        //         invoice = invoice.Id,
        //         CustomerName = customer.Name,
        //         Phone = customer.Phone,
        //         Email = customer.Email,
        //         customerno = order.NoOfCustomers,
        //         TableName = table.Name,
        //         SectionName = Section.Name,
        //         ordersitemdetails = ordersitemdetails,
        //         modifiedOrderDetails = modifiedorderList,
        //         menuItems = MenuItems,
        //     };

        //     return View(viewModel);

        // }

        public IActionResult OrderDetails(int id)
        {
            var order = _context.Orders.FirstOrDefault(i => i.Id == id);



#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            OrderDetailsViewModel vmOrder = _context.Orders
      .Where(o => o.Id == order.Id && o.Isdeleted != true)
      .Select(o => new OrderDetailsViewModel
      {
          OrderDate = o.CreatedAt,
          OrderModifiedDate = o.ModifiedAt,
          CustomerName = o.Customer.Name,
          customerno = o.NoOfCustomers,
          Phone = o.Customer.Phone,
          Email = o.Customer.Email,
          AssignedTables = o.Tableorders.Where(t => t.OrderId == order.Id).Select(tbl => new TableViewModel
          {
              Name = tbl.Table.Name,
          }).ToList(),
          AssignedSection = o.Tableorders.Where(t => t.OrderId == order.Id).Select(sbl => new SectionViewModal
          {
              Name = sbl.Table.Section.Name,
          }).ToList(),
          ordersitemdetails = o.Ordersitemdetails.Where(od => od.OrderId == order.Id && od.Isdeleted != true).Select(od => new OrderItemDetailsViewModel
          {
              ItemsId = od.ItemsId,
              ItemName = od.Items.Name,
              ItemRate = od.Itemrate,
              ItemQuantity = od.Itemquantity,
              modifiedOrderDetails = od.ModifiedOrderDetails.Where(m => m.OrdersItemDetailsId == od.Id).Select(mod => new ModifiedOrderDetailsViewModel
              {
                  ModifierName = mod.ModifierName,
                  Rate = mod.ModifierRate ?? 0,
                  Quantity = mod.Modifierquantity ?? 1,
              }).ToList()
          }).ToList(),
          OrderTaxList = o.OrderTaxes.Where(ot => ot.OrderId == order.Id).Select(ot => new OrderTax
          {
              TaxId = ot.Tax.Id,
              Tax = ot.Tax,
          }).ToList(),
          //   SubTotal = o.,
          //   TotalTax = o.TotalTax,
          //   TotalAmount = o.TotalAmount,
          //   PaymentMode = o.Payments.First().PaymentMethod.ToString()
      })
      .FirstOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            return View(vmOrder);
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