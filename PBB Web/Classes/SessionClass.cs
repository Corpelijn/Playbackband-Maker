using PBB_Web.Classes.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes
{
    public static class SessionClass
    {
        /// <summary>
        /// If session["user"] exist, destroys it and creates a new one
        /// </summary>
        /// <param name="user"></param>
        public static void SetSession(Account user)
        {
            HttpContext.Current.Session["User"] = null;
            HttpContext.Current.Session["User"] = user;

            GetUser().settings.ShowTopBar = true;
        }

        /// <summary>
        /// Check if Session["User"] exist,  If Null => session not set so not logged in return false,  If exists => already logged in return true
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthorized()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the current user in the session
        /// </summary>
        /// <returns></returns>
        public static Account GetUser()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                return (Account)HttpContext.Current.Session["User"];
            }
            else
            {
                return new Account(0, "Anonymous");
            }
        }

        public static void LogOut()
        {
            HttpContext.Current.Session["User"] = null;
            RemovePlaybackband();
        }

        public static void SetPlaybackband(Playbackband pbb)
        {
            HttpContext.Current.Session["PBB"] = null;
            HttpContext.Current.Session["PBB"] = pbb;
        }

        public static void RemovePlaybackband()
        {
            HttpContext.Current.Session["PBB"] = null;
        }

        public static Playbackband GetPlaybackband()
        {
            return (Playbackband)HttpContext.Current.Session["PBB"];
        }

        public static void AddItemToSession(string key, object data)
        {
            HttpContext.Current.Session[key] = data;
        }

        public static object GetItemFromSession(string key)
        {
            return HttpContext.Current.Session[key];
        }

        public static void RemoveItemFromSession(string key)
        {
            HttpContext.Current.Session[key] = null;
        }
    }
}