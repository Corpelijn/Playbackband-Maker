using PBB_Web.Classes.WebInfoDomain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes
{
    public class WebInfo
    {
        private List<DashboardControl> dashboardButtons;
        

        private WebInfo()
        {
            dashboardButtons = new List<DashboardControl>();
        }

        private static WebInfo instance;
        public static WebInfo GetInstance()
        {
            if (instance == null)
            {
                instance = new WebInfo();
                instance.UpdateFromDatabase();
            }

            return instance;
        }

        public void UpdateFromDatabase()
        {
            DataTable dt = Database.DatabaseConnector.GetInstance().ExecuteQuery("select title, icon, description, color, controller, action, access_group from dashboardcontrol");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dashboardButtons.Add(new DashboardControl(dt.Rows[i].ItemArray));
            }
        }

        public List<DashboardControl> GetDashboardControls()
        {
            return new List<DashboardControl>(dashboardButtons);
        }
    }
}