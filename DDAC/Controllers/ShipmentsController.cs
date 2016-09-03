using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DDAC.Models;

namespace DDAC.Controllers
{
    public class ShipmentsController : Controller
    {
        private DDACEntities db = new DDACEntities();

        // GET: Shipments
        //public ActionResult Index()
        //{
        //    var shipments = db.Shipments.Include(s => s.Customer).Include(s => s.Ship).Include(s => s.Yard);
        //    return View(shipments.ToList());
        //}

        public ActionResult Index(int? SelectedYard)
        {
            var yards = db.Yards.ToList();
            ViewBag.SelectedYard = new SelectList(yards, "YardID", "YardName", SelectedYard);
            int yardID = SelectedYard.GetValueOrDefault();
            //var shipments = db.Shipments.Include(s => s.Customer).Include(s => s.Ship).Include(s => s.Yard);
            IQueryable<Shipment> shipments = db.Shipments
                            .Where(s=> !SelectedYard.HasValue||s.YardID==yardID)
                            .Include(s => s.Customer).Include(s => s.Ship).Include(s => s.Yard);
            return View(shipments.ToList());
        }

        // GET: Shipments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // GET: Shipments/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name");
            ViewBag.ShipID = new SelectList(db.Ships, "ShipID", "ShipID");
            ViewBag.YardID = new SelectList(db.Yards, "YardID", "YardName");
            return View();
        }

        // POST: Shipments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShipmentID,Type,InsuredValue,Weight,Destination,Status,ShipID,YardID,CustomerID")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                Customer cust = db.Customers.Find(shipment.CustomerID);
                if (cust.PrepaidCredit > shipment.InsuredValue)
                {
                    cust.PrepaidCredit = cust.PrepaidCredit - (int)shipment.InsuredValue;
                    db.Shipments.Add(shipment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else {
                    return Content("This customer does not have enough money! Press back and try again.");
                }
                            
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", shipment.CustomerID);
            ViewBag.ShipID = new SelectList(db.Ships, "ShipID", "ShipID", shipment.ShipID);
            ViewBag.YardID = new SelectList(db.Yards, "YardID", "YardName", shipment.YardID);
            return View(shipment);
            
        }

        // GET: Shipments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", shipment.CustomerID);
            ViewBag.ShipID = new SelectList(db.Ships, "ShipID", "ShipID", shipment.ShipID);
            ViewBag.YardID = new SelectList(db.Yards, "YardID", "YardName", shipment.YardID);
            return View(shipment);
        }

        // POST: Shipments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShipmentID,Type,InsuredValue,Weight,Destination,Status,ShipID,YardID,CustomerID")] Shipment shipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "Name", shipment.CustomerID);
            ViewBag.ShipID = new SelectList(db.Ships, "ShipID", "ShipID", shipment.ShipID);
            ViewBag.YardID = new SelectList(db.Yards, "YardID", "YardName", shipment.YardID);
            return View(shipment);
        }

        // GET: Shipments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shipment shipment = db.Shipments.Find(id);
            if (shipment == null)
            {
                return HttpNotFound();
            }
            return View(shipment);
        }

        // POST: Shipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shipment shipment = db.Shipments.Find(id);
            db.Shipments.Remove(shipment);
            db.SaveChanges();
            return RedirectToAction("Index");
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
