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
    public class Fare_DetailController : Controller
    {
        private Model_Context db = new Model_Context();

        // GET: Fare_Detail
        public ActionResult Index()
        {
            //Session[u] từ bên AdminController(đọc rồi sẽ biết)
            if (Session["u"] != null)
            {
                var fare_Detail = db.Fare_Detail.Include(f => f.Train_Details);
                return View(fare_Detail.ToList());
            }
            else
            {
                return RedirectToAction("AdmLogin", "Admin");
                //return RedirectToAction("AdmLogin");
            }
        }

        // GET: Fare_Detail/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fare_Detail fare_Detail = db.Fare_Detail.Find(id);
            if (fare_Detail == null)
            {
                return HttpNotFound();
            }
            return View(fare_Detail);
        }

        // GET: Fare_Detail/Create
        public ActionResult Create()
        {
            //Session[u] từ bên AdminController(đọc rồi sẽ biết)
            if (Session["u"] != null)
            {
                ViewBag.Train_Id = new SelectList(db.Train_Details, "Train_Id", "Train_name");
                return View();
            }
            else
            {
                return RedirectToAction("AdmLogin", "Admin");
                //return RedirectToAction("AdmLogin");
            }
        }

        // POST: Fare_Detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Fare_Id,From_City,To_City,Depature_Date,Depature_Time,Train_Id,Class,Price,Total_Passenger")] Fare_Detail fare_Detail)
        {
            //if (fare_Detail.Status.ToString() == "Còn Vé")
            //{
            //    fare_Detail.Status = 1;
            //}
            //else if (fare_Detail.Status.ToString() == "Hết Vé")
            //{
            //    fare_Detail.Status = 0;
            //}
            if (ModelState.IsValid)
            {
                db.Fare_Detail.Add(fare_Detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Train_Id = new SelectList(db.Train_Details, "Train_Id", "Train_name", fare_Detail.Train_Id);
            return View(fare_Detail);
        }

        // GET: Fare_Detail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fare_Detail fare_Detail = db.Fare_Detail.Find(id);
            if (fare_Detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.Train_Id = new SelectList(db.Train_Details, "Train_Id", "Train_name", fare_Detail.Train_Id);
            return View(fare_Detail);
        }

        // POST: Fare_Detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Fare_Id,From_City,To_City,Depature_Date,Depature_Time,Train_Id,Class,Price,Total_Passenger")] Fare_Detail fare_Detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fare_Detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Train_Id = new SelectList(db.Train_Details, "Train_Id", "Train_name", fare_Detail.Train_Id);
            return View(fare_Detail);
        }

        // GET: Fare_Detail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fare_Detail fare_Detail = db.Fare_Detail.Find(id);
            if (fare_Detail == null)
            {
                return HttpNotFound();
            }
            return View(fare_Detail);
        }

        // POST: Fare_Detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fare_Detail fare_Detail = db.Fare_Detail.Find(id);
            db.Fare_Detail.Remove(fare_Detail);
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
