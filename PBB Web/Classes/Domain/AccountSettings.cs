using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class AccountSettings
    {
        public AccountSettings()
        {
            ShowNavbar = false;
        }

        public bool ShowNavbar { get; set; }
    }
}