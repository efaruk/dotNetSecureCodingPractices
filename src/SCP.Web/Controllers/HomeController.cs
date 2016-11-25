using System.Web.Mvc;

namespace SCP.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GeneralCodingPractices()
        {
            return View();
        }

        public ActionResult HowtoDesignSecureSystems()
        {
            return View();
        }

        public ActionResult SystemConfiguration()
        {
            return View();
        }

        public ActionResult DatabaseSecurity()
        {
            return View();
        }
    }
}