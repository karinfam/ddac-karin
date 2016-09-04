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
    public class SchedulesController : Controller
    {
        private DDACEntities db = new DDACEntities();

        // GET: Schedules
        public ActionResult Index()
        {
            var schedules = db.Schedules.Include(s => s.Ship);
            return View(schedules.ToList());
        }
        // GET: Schedules/DisplayDaily
        //public ActionResult DisplayDaily(DateTime? SelectedDate)
        //{
        //    IQueryable<Schedule> schedules= null;
        //    //  DateTime date = SelectedDate.GetValueOrDefault();
        //    if (SelectedDate != null)
        //    {
        //        schedules = db.Schedules
        //                .Where(s => !SelectedDate.HasValue || s.DepartureDateTime == SelectedDate)
        //                .Include(s => s.Ship);
        //    }
        //    else {
        //        schedules = db.Schedules.Include(s => s.Ship);
        //    }

        //    return View(schedules.ToList());
        //}

        public ActionResult DisplayDaily()
        {
            var    schedules = db.Schedules.Include(s => s.Ship);

            return View(schedules.ToList());
        }


        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }
        [Authorize(Roles ="employee")]
        // GET: Schedules/Create
        public ActionResult Create()
        {
            ViewBag.ShipID = new SelectList(db.Ships, "ShipID", "ShipID");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleID,Destination,DepartureDateTime,ArrivalDateTime,ShipID")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ShipID = new SelectList(db.Ships, "ShipID", "ShipID", schedule.ShipID);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        [Authorize(Roles = "employee")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShipID = new SelectList(db.Ships, "ShipID", "ShipID", schedule.ShipID);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleID,Destination,DepartureDateTime,ArrivalDateTime,ShipID")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ShipID = new SelectList(db.Ships, "ShipID", "ShipID", schedule.ShipID);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        [Authorize(Roles = "employee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [Authorize(Roles = "employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
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
