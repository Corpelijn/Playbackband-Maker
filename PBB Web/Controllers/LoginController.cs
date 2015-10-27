using PBB_Web.Classes;
using PBB_Web.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBB_Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (Account.ValidateCredentials(username, password))
            {
                SessionClass.SetSession(Account.GetAccountFromDatabase(username));
                return RedirectToAction("main", "playbackband");
            }

            return View();
        }
	}
}