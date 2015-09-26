using PBB_Web.Classes.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PBB_Web.Controllers
{
    public class PlaybackbandController : Controller
    {
        public ActionResult Index()
        {
            DatabaseReader.ReadDatabase();
            return View();
        }

        public ActionResult Main()
        {
            return View();
        }
	}
}