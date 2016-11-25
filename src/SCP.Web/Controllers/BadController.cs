using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCP.Data;
using SCP.Data.Entities;
using SCP.Diagnostics.Logging;
using SCP.Web.Models;

namespace SCP.Web.Controllers
{
    public class BadController : Controller
    {
        private readonly ILogger _logger;
        private readonly ScpDbContext _context;

        public BadController(ILogger logger, ScpDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult InputValidation()
        {
            // QueryString Parameters Remembered by Browsers
            var name = Request.QueryString["name"];
            var ageTemp = Request.QueryString["age"];
            var age = string.IsNullOrEmpty(ageTemp) ? 0 : int.Parse(ageTemp);
            var notes = Request.QueryString["notes"];
            var model = new InputValidationModel(name, age, notes);
            return View(model);
        }

        public ActionResult OutputEncoding()
        {
            var model = new OutputEncodingModel();
            return View(model);
        }

        public ActionResult Authentication()
        {
            User user = null;
            var username = Request.QueryString["username"];
            var password = Request.QueryString["password"];
            if (!string.IsNullOrEmpty(username))
            {
                // ce0bfd15059b68d67688884d7a3d3e8c
                var query = "SELECT * FROM User WHERE username ='" + username + "' and password ='" + password + "'";
                user = _context.Users.SqlQuery(query).FirstOrDefault();
            }
            if (user == null) user = new User();
            return View(user);
        }

        private const string SessionCookieName = "obviously_session_id";

        public ActionResult SessionManagement()
        {
            var siteCookie = GetSessionCookie();
            if (siteCookie != null)
            {
                ViewData["session_id"] = siteCookie.Value;
                return View();
            }
            var rnd = new Random();
            var random = rnd.Next(0, 1000);
            var sessionId = (DateTime.Now.Ticks + random).ToString();
            var sessionCookie = new HttpCookie(SessionCookieName)
            {
                Value = sessionId
            };
            Response.Cookies.Add(sessionCookie);

            ViewData["session_id"] = sessionId;
            return View();
        }

        private HttpCookie GetSessionCookie()
        {
            if (Request.Cookies.AllKeys.Contains(SessionCookieName))
            {
                var cookie = Request.Cookies[SessionCookieName];
                return cookie;
            }
            return null;
        }

        public ActionResult SessionManagementLogout(bool logout)
        {
            if (logout)
            {
                Session.Abandon();
                ViewData["session_id"] = "good bye";
            }
            return RedirectToAction("SessionManagement");
        }

        private const string AccessLevel = "access_level";

        public ActionResult AccessControl(int userType = 0)
        {
            ViewData[AccessLevel] = "None";
            switch (userType)
            {
                case 1:
                    ViewData[AccessLevel] = "User";
                    break;
                case 2:
                    ViewData[AccessLevel] = "Staff";
                    break;
                case 3:
                    ViewData[AccessLevel] = "Admin";
                    break;
            }
            return View();
        }

        public ActionResult CryptographicPractices()
        {
            return View();
        }


        public ActionResult ErrorHandling(string someParameter = "")
        {
            Exception exception;
            try
            {
                throw new InvalidOperationException(string.Format("Invalid parameter {0}", someParameter));
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return View(exception);
        }

        public ActionResult Logging(string someSensitiveData = "")
        {
            try
            {
                throw new InvalidOperationException(string.Format("Invalid parameter {0}", someSensitiveData));
            }
            catch (Exception ex)
            {
                _logger.Error("Error Happend", ex);
            }
            return View();
        }

        public ActionResult DataProtection()
        {
            return View();
        }

        public ActionResult CommunicationSecurity()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FileManagement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FileManagement(HttpPostedFileBase upload)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(upload.FileName);
                var path = Path.Combine(Server.MapPath("~/uploads/"), fileName);
                upload.SaveAs(path);
            }
            return View();
        }

        public ActionResult MemoryManagement()
        {
            var fs = new FileInfo(Server.MapPath("~/App_Data/test.txt")).OpenRead();
            var sr = new StreamReader(fs);
            ViewData["data"] = sr.ReadToEnd();
            return View();
        }
    }
}