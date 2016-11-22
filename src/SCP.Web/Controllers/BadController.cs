using System.Web.Mvc;
using SCP.Web.Models;
using SCP;

namespace SCP.Web.Controllers
{
    public class BadController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult InputValidation()
        {
            // QueryString Parameters Remembered by Browsers
            string name = Request.QueryString["name"];
            string ageTemp = Request.QueryString["age"];
            int age = string.IsNullOrEmpty(ageTemp) ? 0 : int.Parse(ageTemp);
            string notes = Request.QueryString["notes"];
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
            string username = Request.QueryString["username"];
            string password = Request.QueryString["password"];
            password = password.ToMd5();

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