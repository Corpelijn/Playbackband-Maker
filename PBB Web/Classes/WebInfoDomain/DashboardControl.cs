﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.WebInfoDomain
{
    public class DashboardControl
    {
        private string title;
        private string icon;
        private string description;
        private string color;
        private string controller;
        private string action;
        private int access;

        public DashboardControl(object[] data)
        {
            title = (string)(data[0] == DBNull.Value ? "" : data[0]);
            icon = (string)(data[1] == DBNull.Value ? "" : data[1]);
            description = (string)(data[2] == DBNull.Value ? "" : data[2]);
            color = (string)(data[3] == DBNull.Value ? "" : data[3]);
            controller = (string)(data[4] == DBNull.Value ? "" : data[4]);
            action = (string)(data[5] == DBNull.Value ? "" : data[5]);
            access = (int)(data[6] == DBNull.Value ? 0 : data[6]);
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        public int Access
        {
            get { return access; }
            set { access = value; }
        }
    }
}