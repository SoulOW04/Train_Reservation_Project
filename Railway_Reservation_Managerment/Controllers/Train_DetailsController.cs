using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Railway_Reservation_Managerment.Models;

namespace Railway_Reservation_Managerment.Controllers
{
    public class Train_DetailsController : Controller
    {
        private Model_Context db = new Model_Context();

        // GET: Train_Details
        public ActionResult Index()
        {
            //Session[u] từ bên AdminController(đọc rồi sẽ biết)
            if (Session["u"] != null)
            {
                return View(db.Train_Details.ToList());
            }
            else
            {
                return RedirectToAction("AdmLogin", "Admin");
                //return RedirectToAction("AdmLogin");
            }
        }

        // GET: Train_Details/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train_Details train_Details = db.Train_Details.Find(id);
            if (train_Details == null)
            {
                return HttpNotFound();
            }
            return View(train_Details);
        }

        // GET: Train_Details/Create
        public ActionResult Create()
        {
            //Session[u] từ bên AdminController(đọc rồi sẽ biết)
            if (Session["u"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdmLogin", "Admin");
                //return RedirectToAction("AdmLogin");
            }
        }

        // POST: Train_Details/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Train_Id,Train_name,No_of_compartment,Seats_Available,Train_Type")] Train_Details train_Details)
        {
            if (ModelState.IsValid)
            {
                db.Train_Details.Add(train_Details);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(train_Details);
        }

        // GET: Train_Details/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train_Details train_Details = db.Train_Details.Find(id);
            if (train_Details == null)
            {
                return HttpNotFound();
            }
            return View(train_Details);
        }

        // POST: Train_Details/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Train_Id,Train_name,No_of_compartment,Seats_Available,Train_Type")] Train_Details train_Details)
        {
            if (ModelState.IsValid)
            {
                db.Entry(train_Details).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(train_Details);
        }

        // GET: Train_Details/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train_Details train_Details = db.Train_Details.Find(id);
            if (train_Details == null)
            {
                return HttpNotFound();
            }
            return View(train_Details);
        }

        // POST: Train_Details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Train_Details train_Details = db.Train_Details.Find(id);
            var n = from t in db.Fare_Detail where t.Train_Id == id select t;
            db.Fare_Detail.RemoveRange(n);
            db.Train_Details.Remove(train_Details);
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
