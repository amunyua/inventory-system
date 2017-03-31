using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InventoryDataEntities.Models;

namespace InventoryManagementSystemMVC.Controllers
{
    public class StockController : Controller
    {
        private IMSDataEntities db = new IMSDataEntities();
        private List<string> Errors = new List<string>();

        // GET: Stock
        public ActionResult Index()
        {
            var warehouseProducts = db.WarehouseProducts.Include(w => w.ProdId).Include(w => w.Warehouses);
            return View(warehouseProducts.ToList());
        }

        // GET: Stock/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarehouseProduct warehouseProduct = db.WarehouseProducts.Find(id);
            if (warehouseProduct == null)
            {
                return HttpNotFound();
            }
            return View(warehouseProduct);
        }

        // GET: Stock/Create
        public ActionResult Create()
        {
            ViewBag.ProductId = new SelectList(db.Products, "Product_id", "ProductCode");
            ViewBag.WarehouseId = new SelectList(db.Warehouses, "Id", "WarehouseName");
            return View();
        }

        // POST: Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection request )
        {
            //here we do validation first
            Validate("TransactionType,TransactionCategory,SupplierId,WarehouseId,ProductId", request);
            if (Errors.Any())
            {
                ViewBag.Errors = Errors;
                return View(request);
            }
           
            var transaction = new Transactions()
            {
                DeliveryNote = request["DeliveryNoteRefNo"],
                InvoiceRefNumber = request["InvoiceRefNumber"],
                TransactionType = request["TransactionType"],
                TransactionCategory = request["TransactionCategory"],
                Status = true,
                TransactionDate = DateTime.Now,
                SupplierId = Int64.Parse(request["SupplierId"]),
            };
            switch (request["TransactionType"])
            {
                case "PURCHASE":
                    if (request["TransactionCategory"] == "fuel")
                    {
                        if (Validate("DriverId,VehicleId", request))
                        {
                            transaction.DriverId = Int64.Parse(request["DriverId"]);
                            transaction.VehicleId = Int64.Parse(request["VehicleId"]);
                            if (Validate("ExpectedQuantity,ActualQuantityAvailable,DipBeforeOffload,DipAfterOffload,AmountSoldDuringOffload,AmountPerLiter", request))
                            {
                                db.FuelTransactionDetails.Add(new FuelTransactionDetails()
                                {
                                    TransactionId = transaction.Id,
                                    ExpectedQuantity = double.Parse(request["ExpectedQuantity"]),
                                    ActualQuantityAvailable = double.Parse(request["ActualQuantityAvailable"]),
                                    DipBeforeOffload = double.Parse(request["DipBeforeOffload"]),
                                    DipAfterOffload = double.Parse(request["DipAfterOffload"]),
                                    AmountSoldDuringOffload = double.Parse(request["AmountSoldDuringOffload"]),
                                    PricePerLiter = double.Parse(request["PricePerLiter"])
                                });
                            }
                        }
                    }
                    break;

            }
            db.Transactions.Add(transaction);
            db.TransctionDetails.Add(new TransactionDetails()
            {
                TransactionId = transaction.Id,
                ProductId = Int64.Parse(request["ProductId"]),
                WarehouseId = Int64.Parse(request["WarehouseId"]),
//                Quantity = double.Parse(request["ActualQuantityAvailable"])
            });
            if (Errors.Any())
            {
                ViewBag.Errors = Errors;
                return View(request);
            }
            else
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            

        }

      

        //get all the products belonging to a certain warehouse
        [HttpPost]
        public JsonResult WarehouseProducts()
        {
            var warehouseId = Int64.Parse(Request.Form["warehouseId"]);
            var data = db.WarehouseProducts.Where(x => x.WarehouseId == warehouseId);
            return Json(data);
        }


        [HttpPost]
        public JsonResult SupplierVehicles()
        {
            var supplierId = Int64.Parse(Request.Form["supplierId"]);

            
            var vehicles = db.Vehicles.Where(x => x.SupplierId == supplierId);
            
            return Json(vehicles);
        }

        [HttpPost]
        public JsonResult SupplierDrivers()
        {
            var supplierId = Int64.Parse(Request.Form["supplierId"]);


            var drivers = db.Drivers.Where(d => d.SupplierId == supplierId);

            return Json(drivers);
        }

        public bool Validate(string attr, FormCollection request)
        {
            bool status = true;
            var values = attr.Split(',');
            foreach (var value in values)
            {
                if (request[value] == "")
                {
                    status = false;
                    Errors.Add(value + " Is Required");
                }
            }
           
            return status;
        }

        public ActionResult Transactions()
        {
            var transactions = db.Transactions;
            return View(transactions);
        }

      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
