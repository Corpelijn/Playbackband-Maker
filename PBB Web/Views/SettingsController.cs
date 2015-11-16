using PBB_Web.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBB_Web.Views
{
    public class SettingsController : Controller
    {
        public ActionResult Index()
        {
            SessionClass.GetUser().settings.ShowNavbar = true;
            return View();
        }
	}
}