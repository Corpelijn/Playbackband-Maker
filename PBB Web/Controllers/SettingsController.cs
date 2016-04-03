using PBB_Web.Classes;
using PBB_Web.Classes.Database;
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

        public ActionResult Account()
        {
            return View();
        }

        public ActionResult Rechten()
        {
            SessionClass.GetUser().settings.ShowNavbar = true;
            return View();
        }

        [HttpPost]
        public ActionResult Rechten(string code, string description)
        {
            DatabaseConnector.GetInstance().ExecuteNonQuery("insert into recht (code, beschrijving) values (\'" + code + "\',\'" + description + "\')");
            return View();
        }

        [HttpPost]
        public ActionResult Taal(string taal, string afkorting)
        {
            DatabaseConnector.GetInstance().ExecuteNonQuery("insert into taal (taal, afkorting) values (\'" + taal + "\',\'" + afkorting + "\')");
            return RedirectToAction("index", "settings");
        }
	}
}