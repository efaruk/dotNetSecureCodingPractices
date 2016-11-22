using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCP.Web.Models;

namespace SCP.Web.Controllers
{
    public class GoodController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult InputValidation()
        {
            return View(new InputValidationModel());
        }

        [HttpPost]
        public ActionResult InputValidation(InputValidationModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Your inputs are not valid.");
                return View(new InputValidationModel());
            }
            return View(model);
        }

        public ActionResult OutputEncoding()
        {
            var model = new OutputEncodingModel();
            // Sanitize Path
            if (model.Path.Contains(HttpRuntime.AppDomainAppPath))
            {
                model.Path = "~\\";
            }
            // Sanitize Credit Card Number
            var first4 = model.CreditCardNumber.Substring(0, 4);
            var last4 = model.CreditCardNumber.Substring(model.CreditCardNumber.Length - 4, 4);
            model.CreditCardNumber = first4 + " **** **** " + last4;
            return View(model);
        }

        public ActionResult Authentication()
        {
            return View();
        }

        public ActionResult PasswordManagement()
        {
            return View();
        }

        public ActionResult SessionManagement()
        {
            return View();
        }

        public ActionResult AccessControl()
        {
            return View();
        }

        public ActionResult CryptographicPractices()
        {
            return View();
        }

        public ActionResult ErrorHandling()
        {
            return View();
        }

        public ActionResult Logging()
        {
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

        public ActionResult DatabaseSecurity()
        {
            return View();
        }

        public ActionResult FileManagement()
        {
            return View();
        }

        public ActionResult MemoryManagement()
        {
            return View();
        }
    }
}