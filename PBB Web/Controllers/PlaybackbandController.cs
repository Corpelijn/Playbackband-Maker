using PBB_Web.Classes;
using PBB_Web.Classes.Database;
using PBB_Web.Classes.Domain;
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
            //DatabaseReader.ReadDatabase();
            return View();
        }

        public ActionResult Main()
        {
            SessionClass.GetUser().settings.ShowNavbar = true;
            return View();
        }

        [HttpPost]
        public ActionResult Main(int id)
        {
            DatabaseReader.ReadPlaybackbandFromDatabase(id);
            return Redirect("Index");
        }

        public ActionResult Toevoegen()
        {
            SessionClass.GetUser().settings.ShowNavbar = true;
            return View();
        }

        [HttpPost]
        public ActionResult Toevoegen1(DateTime dag1, DateTime dag2, int blokken, string intro, string finale)
        {
            Playbackband pbb = new Playbackband(dag1, dag2, blokken, intro != null, finale != null);
            SessionClass.SetPlaybackband(pbb);

            return Redirect("Toevoegen2");
        }

        public ActionResult DownloadExcel()
        {
            byte[] file = ExcelSheetExporter.GenerateExcel();

            Response.AddHeader("Content-Disposition", "attachment; filename=ExcelFile.xlsx");
            return new FileContentResult(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public ActionResult Toevoegen2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Toevoegen2(params string[] parameters)
        {
            foreach (Blok b in Playbackband.Instance.Blokken)
            {
                b.SetAantal(Convert.ToInt32(Request["blok" + b.index]));
            }
            Playbackband.Instance.SetValid();
            return RedirectToAction("", "playbackband");
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexEdit(string blok, string fragment)
        {
            SessionClass.AddItemToSession("tempEditFragment", Playbackband.Instance.Blokken[Convert.ToInt32(blok)].fragmenten[Convert.ToInt32(fragment)]);
            return RedirectToAction("edit", "playbackband");
        }
	}
}