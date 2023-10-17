using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Railway_Reservation_Managerment.Models;
using System.Data.Entity.Validation;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;

namespace Railway_Reservation_Managerment.Controllers
{
    public class PassengersController : Controller
    {
        private Model_Context db = new Model_Context();

        // GET: Passengers
        

        public string encryption(String password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }

        public JsonResult IsEmailExist(string eMail)
        {
            return Json(!db.Passengers.Any(x => x.Email == eMail), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsUserNameExist(string Login_Name)
        {
            return Json(!db.Passengers.Any(x => x.Login_Name.Trim() == Login_Name.Trim()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsUserPhoneExsist(string Phone)
        {
            return Json(!db.Passengers.Any(x => x.Phone == Phone), JsonRequestBehavior.AllowGet);
        }

        // GET: Passengers/Create
       

        // POST: Passengers/Delete/5
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Register(int id = 0)
        {

            Passenger ps = new Passenger();
            return View(ps);
        }
        Model_Context mod = new Model_Context();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Passenger passenger, string userdata)
        {
            
            if (ModelState.IsValid)
            {
                //string passwordHash = BCrypt.Net.BCrypt.HashPassword(passenger.Login_Password);
                try
                {
                    //passenger.Login_Password = BCrypt.Net.BCrypt.HashPassword(passenger.Login_Password);
                    passenger.Login_Password = encryption(passenger.Login_Password);
                    passenger.Confirm_Password = passenger.Login_Password;
                    //passenger.Confirm_Password = BCrypt.Net.BCrypt.HashPassword(passenger.Login_Password);
                    db.Passengers.Add(passenger);
                    db.SaveChanges();
                    TempData["message"] = "Register Successfully";
                    return RedirectToAction("Login");
                }
                catch (DbEntityValidationException ex)
                {
                    int i = 0;
                    i++;
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }

            }

            return View(passenger);
        }

        public void SendVerificationLinkEmail(string Email, string activationCode, string emailFor = "ResetPassword")
        {

            int i = 0;
            var verifyUrl = "/Passengers/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("conganhforwork@gmail.com", "Cong Anh Awesome");
            var toEmail = new MailAddress(Email);
            var fromEmailPassword = "kanhdepzai123."; //thay thay thế vào password thật


            string subject = "";
            string body = "";

            if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi <br/><br/> We Got request for reset your account password." +
                    ".Please click on the below link to Reset your password " +
                    "</br></br><a href=" + link + "> Reset Password Link</a>";

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

        public ActionResult ForgotPassword()
        {
            int i = 0;
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            //Verify Email ID
            //generate reset password link
            //send email
            int i = 0;
            string message = "";
            //bool status = false;
            using (Model_Context md = new Model_Context())
            {
                var account = md.Passengers.Where(a => a.Email == email).FirstOrDefault();
                if (account != null)
                {
                    //send email or reset password 
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;


                    md.Configuration.ValidateOnSaveEnabled = false;
                    md.SaveChanges();
                    message = "Reset password link has been sent to your email Id";
                }
                else
                {
                    message = "Account not found";

                }
                ViewBag.Message = message;
                return View();
            }
        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page 

            using (Model_Context md = new Model_Context())
            {


                var user = md.Passengers.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {

            var message = "";
            if (ModelState.IsValid)
            {
                using (Model_Context md = new Model_Context())
                {

                    var user = md.Passengers.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Login_Password = encryption(model.NewPassword);
                        user.ResetPasswordCode = "";

                        md.Configuration.ValidateOnSaveEnabled = false;
                        md.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something Invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }

        public ActionResult Login()
        {
            //ViewBag.mss = TempData["message"];
            int i = 0;
            ViewBag.msg = "";
            return View();
        }

        [HttpPost]
        public ActionResult Login(Passenger passenger)
        {
            using (Model_Context db = new Model_Context())
            {
                int i = 0;
                //var AssNo = db.Passengers.Single(a => a.Login_Name == passenger).AssessmentName;
                //var result = gateway.Where(x => x.TestName == something).Select(x => x.ID);
                passenger.Login_Password = encryption(passenger.Login_Password);

                var pgrs = db.Passengers.Where(p => p.Login_Name == passenger.Login_Name && p.Login_Password == passenger.Login_Password);
                if (pgrs.Count() > 0)
                {
                    foreach (var item in pgrs)
                    {
                        Session["PassengerID"] = item.Login_Id.ToString();
                        Session["PassengerName"] = item.Login_Name.ToString();
                        Session["Email"] = item.Email.ToString();
                        Session["Phone"] = item.Phone.ToString();
                    }

                    if (Session["c"] != null && Session["PassengerID"] != null)
                    {
                        //int i = 0;
                        //string a = Session["TotalPrice"].ToString();
                        return RedirectToAction("Booking", "Train_Booking", new
                        {
                            @Lid = Session["PassengerID"],
                            @Fid = Session["FareId"],
                            @Email = Session["Email"],
                            @Phone = Session["Phone"],
                            @From = Session["From"],
                            @To = Session["To"],
                            @DepartureDate = Session["DepartureDate"],
                            @TotalPrice = Session["TotalPrice"],
                            @Class = Session["Class"]
                        });
                    }
                    else
                    {
                        return RedirectToAction("IndexSelectFare", "UserForm");
                    }
                }
                else
                {
                    ViewBag.msg = "Invalid UserName or Password";
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index","UserForm");
        }

        //validate UserName Already exist or not
        public JsonResult IsUserNameExists(string Login_Name)
        {
            return Json(!db.Passengers.Any(y => y.Login_Name == Login_Name), JsonRequestBehavior.AllowGet);
        }

        //validate Email Already exist or not
        public JsonResult IsEmailExists(string Email)
        {
            return Json(!db.Passengers.Any(x => x.Email == Email), JsonRequestBehavior.AllowGet);
        }

    }
}
