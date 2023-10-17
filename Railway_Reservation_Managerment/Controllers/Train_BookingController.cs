using Railway_Reservation_Managerment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Railway_Reservation_Managerment.Controllers
{
    public class Train_BookingController : Controller
    {
        // GET: Train_Booking

        // GET: TrainBook
        Model_Context db = new Model_Context();

        public ActionResult Booking(int Lid, int Fid, string Email, int Phone, string From, string To, string DepartureDate, int TotalPrice, string Class)
        {
            int i = 0;

            Session["T"] = Lid;
            Session["F"] = Fid;
            ViewBag.Tp = Session["TrainPas"];
            return View();
        }
        [HttpPost]
        public ActionResult Booking(Train_Booking trl)
        {
            int i = 0;
            string tp = Session["T"].ToString();
            string fi = Session["F"].ToString();
            //string i = Session["TrainPas"].ToString();

            int tt = Convert.ToInt32(tp);
            int ff = Convert.ToInt32(fi);
            //int ii = Convert.ToInt32(i);

            var t = db.Passengers.Where(al => al.Login_Id == tt).SingleOrDefault();
            var f = db.Fare_Detail.Where(al => al.Fare_Id == ff).SingleOrDefault();
            //add to db
            //int kk = 0;
            if (t !=null && f!= null)
            {
                trl.Login_Id = t.Login_Id;
                trl.Fare_Id = f.Fare_Id;
                db.Train_Booking.Add(trl);
                
                db.SaveChanges();

                var n = db.Train_Booking.Where(al => al.Name == trl.Name);

                //Lấy name để chuyền vào email
                foreach (var item in n)
                {
                    Session["Name"] = item.Name.ToString();
                    Session["PaymentType"] = item.PaymentType.ToString();
                }
                ViewBag.mss = "Book succes";

                
                
            }
            //add to email
            
            string message = "";
            string email = Session["Email"].ToString();
            var account = db.Passengers.Where(a => a.Email == email).FirstOrDefault();
            if (account != null)
            {
                //send email or reset password 
                string activitation = Guid.NewGuid().ToString();
                SendUserPaymentInfo(account.Email, activitation, "Payment");
                account.PurchaseCode = activitation;


                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "Purchase information has been sent to your email !";
            }
            else
            {
                message = "Account not found";

            }
            ViewBag.Message = message;

            return RedirectToAction("Index");
        }
        //gửi email về máy 
        public void SendUserPaymentInfo(string Email, string activationCode, string emailFor = "Payment")
        {

            int i = 0;
            var verifyUrl = "/Passengers/" + emailFor + "/" + activationCode;
            //var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("titcongminh2004@gmail.com", "PayMent Message");
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "kanhdepzai123."; //thay thay thế vào password thật


            string subject = "";
            string body = "";
            if (emailFor == "Payment")
            {
                subject = "Payment";
                body = "Hi <br/><br/> This is your Purchase information " + "<br/><br/>" +
                    "Full Name: " + Session["Name"] + "<br/>" +
                    "Email: " + Session["Email"] + "<br/>" +
                    "Phone: " + Session["Phone"] + "<br/>" +

                    "<br/><br/> Order Ticket From: " + Session["From"] + " To " + Session["To"] + "<br/>" +
                    "Departure Date: " + Session["DepartureDate"] + "<br/>" + " Class: " + Session["Class"] + "<br/>" +
                    " Paytype: " + Session["PaymentType"]  + 
                    "<br/><br/>Total Price to pay(including VAT): " + Session["TotalPrice"] + "<br/>";

            }
            else
            {
                subject = "Wrong";
                body = "something when wrong !";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };


            using (var Message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            }) smtp.Send(Message);


        }
        public ActionResult Index()
        {
            int i = 0;
            //ViewBag.mm = Session["T"];
            ViewBag.mm = Session["PassengerID"];
            int i2 =Convert.ToInt32(ViewBag.mm);
            Train_Booking tr = new Train_Booking();
            var query = from c in db.Train_Booking where c.Login_Id == i2 select c;
            if (query != null)
            {
                return View(query.ToList());
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train_Booking tr = db.Train_Booking.Find(id);
            if (tr == null)
            {
                return HttpNotFound();
            }
            return View(tr);
        }

        public ActionResult Cancellation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Train_Booking tr = db.Train_Booking.Find(id);
            if (tr == null)
            {
                return HttpNotFound();
            }
            return View(tr);
        }

        // POST: Fare_Detail/Delete/5
        [HttpPost, ActionName("Cancellation")]
        [ValidateAntiForgeryToken]
        public ActionResult CancellationConfirmed(int id)
        {
            Train_Booking tr = db.Train_Booking.Find(id);
            db.Train_Booking.Remove(tr);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}