using Railway_Reservation_Managerment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Railway_Reservation_Managerment.Controllers
{
    public class UserFormController : Controller
    {
        // GET: UserForm
        Model_Context db = new Model_Context();
        // GET: TrainBook
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexSelectFare()
        {
            ViewBag.fcity = db.Fare_Detail.Select(al => al.From_City).Distinct().ToList();
            ViewBag.tcity = db.Fare_Detail.Select(al => al.To_City).Distinct().ToList();
            ViewBag.clas = db.Fare_Detail.Select(al => al.Class).Distinct().ToList();
            return View();
        }
        //
        //public ActionResult Searchs()
        //{
        //    int i = 0;
            
        //    return View();
        //}

        [HttpPost]
        public ActionResult Searchs(string cityFrom, string cityTo, DateTime date, string Class)
        {
            int i = 0;
            //i++;
            var c = db.Fare_Detail.Where(al => al.From_City == cityFrom || al.To_City == cityTo || al.Depature_Date == date || al.Class == Class);
            if (c.Count() > 0)
            {
                foreach (var item in c)
                {
                    Session["FareId"] = item.Fare_Id.ToString();
                    Session["TotalPrice"] = item.Price.ToString();
                    Session["DepatureDate"] = item.Depature_Date.ToString();
                }
                Session["c"] = c;
                //ViewBag.C = Session["c"];
                ViewBag.C = c;
                return View();
            }
            else
            {
                return HttpNotFound();
            }
            
        }
        //[HttpPost]
        //public ActionResult Search(string cityFrom, string cityTo, string date, string Class)
        //{
        //    var c = db.Fare_Detail.Where(al => al.From_City.Equals(cityFrom) && al.To_City.Equals(cityTo) && al.Depature_Date.Equals(date) && al.Class.Equals(Class));
        //    ViewBag.ss = c;
        //    return View();
        //}

        public ActionResult IndexAboutUs()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

    }
}