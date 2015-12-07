using PBB_Web.Classes;
using PBB_Web.Classes.Database;
using PBB_Web.Classes.Exporters.Files;
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
            SessionClass.GetUser().settings.ShowNavbar = false;
            DatabaseReader.ReadDatabase();
            return View();
        }

        public ActionResult Main()
        {
            SessionClass.GetUser().settings.ShowNavbar = true;
            return View();
        }

        public ActionResult Toevoegen()
        {
            SessionClass.GetUser().settings.ShowNavbar = true;
            return View();
        }

        public ActionResult DownloadExcel()
        {
            byte[] file = ExcelSheetExporter.GenerateExcel();

            Response.AddHeader("Content-Disposition", "attachment; filename=ExcelFile.xlsx");
            return new FileContentResult(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

	}
}