using System;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SCP.Configuration;
using SCP.Data;
using SCP.Data.Entities;
using SCP.Diagnostics.Logging;
using SCP.Security;
using SCP.Security.Cryptography;
using SCP.Web.Models;

namespace SCP.Web.Controllers
{
    [AllowAnonymous]
    public class GoodController : Controller
    {
        private readonly ILogger _logger;
        private readonly ISettingsProvider _settingsProvider;
        private readonly IScpUnitOfWork _unitOfWork;
        private readonly IPasswordHashProvidcer _passwordHashProvidcer;
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private string _key;
        private string _vector;
        private string _salt;

        public GoodController(ILogger logger, ISettingsProvider settingsProvider, IScpUnitOfWork unitOfWork, IPasswordHashProvidcer passwordHashProvidcer, IEncryptionProvider encryptionProvider, IRandomNumberGenerator randomNumberGenerator)
        {
            _logger = logger;
            _settingsProvider = settingsProvider;
            _unitOfWork = unitOfWork;
            _passwordHashProvidcer = passwordHashProvidcer;
            _encryptionProvider = encryptionProvider;
            _randomNumberGenerator = randomNumberGenerator;

            _key = _settingsProvider.GetAppSetting(ConfigurationKeys.Key);
            _vector = _settingsProvider.GetAppSetting(ConfigurationKeys.Vector);
            _salt = _settingsProvider.GetAppSetting(ConfigurationKeys.Salt);
        }

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
        [ValidateAntiForgeryToken]
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
                model.Path = "~\\";
            // Sanitize Credit Card Number
            var first4 = model.CreditCardNumber.Substring(0, 4);
            var last4 = model.CreditCardNumber.Substring(model.CreditCardNumber.Length - 4, 4);
            model.CreditCardNumber = first4 + " **** **** " + last4;
            return View(model);
        }

        [HttpGet]
        public ActionResult Authentication()
        {
            var model = new AuthenticationViewModel();
            return View(model);
        }

        private const string SessionUserKey = "User";
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Authentication(AuthenticationPostModel postModel)
        {
            var model = new AuthenticationViewModel();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userName = postModel.Username;
            var password = _passwordHashProvidcer.Hash(postModel.Password, _salt);
            var user = _unitOfWork.UserRepository.GetUserByUsernameAndPassword(userName, password);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                // Not a good practice
                Session[SessionUserKey] = user;
                // Map User to Model
                model = new AuthenticationViewModel()
                {
                    IdHashed = _encryptionProvider.Encrypt(user.Id.ToString(), _key, _vector),
                    Name = user.Name,
                    EMail = user.EMail.SanitizeEmail()
                };
            }
            return View(model);
        }

        private const string SessionCookieNamePrefix = "site__";
        public ActionResult SessionManagement()
        {
            var siteCookie = GetSessionCookie();
            if (siteCookie != null)
            {
                ViewData["session_id"] = siteCookie.Value;
                return View();
            }
            var sessionId = _passwordHashProvidcer.Hash(Guid.NewGuid() + _randomNumberGenerator.GenerateLong().ToString(), _salt);
            var custom = _passwordHashProvidcer.Hash(Guid.NewGuid() + _randomNumberGenerator.Generate().ToString(), _salt);
            var cookieName = string.Format("{0}{1}", SessionCookieNamePrefix, custom);
            var sessionCookie = new HttpCookie(cookieName)
            {
                Value = sessionId,
                Domain = Request.Url.Host,
                Path = "~/good/",
                // Expires = End of the session,
                HttpOnly = true,
                Shareable = false
            };
            Response.Cookies.Add(sessionCookie);
            ViewData["session_id"] = sessionId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SessionManagement(bool logout)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                var sc = GetSessionCookie();
                if (sc != null)
                {
                    var removeCookie = new HttpCookie(sc.Name)
                    {
                        Path = "~/good/", // If not same path provided cookie will not be removed
                        Expires = DateTime.Now.AddDays(-1) // To remove cookie: set to expire
                    };
                    Response.Cookies.Add(removeCookie);
                }
                ViewData["session_id"] = "bye bye...";
            }
            else
            {
                ViewData["session_id"] = "You are a terrible lier!";
            }
            return View();
        }

        private HttpCookie GetSessionCookie()
        {
            var key = Request.Cookies.AllKeys.AsQueryable().FirstOrDefault(s => s.StartsWith(SessionCookieNamePrefix));
            if (string.IsNullOrEmpty(key)) key = SessionCookieNamePrefix;
            var cookie = Request.Cookies[key];
            return cookie;
        }

        private const string AccessLevel = "access_level";
        public ActionResult AccessControl()
        {
            ViewData[AccessLevel] = "None";
            if (Session[SessionUserKey] == null) return View();

            // Not a good practice
            var user = (User)Session[SessionUserKey];
            if (user.IsAdmin)
            {
                ViewData[AccessLevel] = "Admin";
            }
            else if (user.IsStaff)
            {
                ViewData[AccessLevel] = "Staff";
            }
            else
            {
                ViewData[AccessLevel] = "User";
            }
            return View();
        }

        public ActionResult CryptographicPractices()
        {
            return View();
        }

        public ActionResult ErrorHandling()
        {
            try
            {
                throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                _logger.Error("We are handling don't worry", ex);
            }
            return View();
        }


        public ActionResult Logging()
        {
            if (Session[SessionUserKey] != null)
            {
                var user = (User)Session[SessionUserKey];
                _logger.Info(string.Format("{0} called Logging action.", user.UserName));
            }

            _logger.Debug(">> Logging");
            try
            {
                _logger.Debug(">> Try");
                _logger.Info(":: Throwing InvalidOperationException");
                _logger.Debug("<< Try");
                throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                ViewData["incident_id"] = _encryptionProvider.Encrypt(Guid.NewGuid().ToString() + _randomNumberGenerator.Generate(), _key, _vector);
                _logger.Debug(">> Catch");
                _logger.Error("Log Everything", ex);
                _logger.Debug("<< Catch");
            }
            _logger.Debug("<< Logging");
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

        public ActionResult FileManagement()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileManagement(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var fileName = Guid.NewGuid() + "__" + Path.GetFileName(upload.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    upload.SaveAs(path);
                }
            }
            return View();
        }

        public ActionResult MemoryManagement()
        {
            return View();
        }
    }
}