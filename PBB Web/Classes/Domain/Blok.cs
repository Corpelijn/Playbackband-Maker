﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Blok : IComparable
    {
        public int id;
        public Playbackband parent;
        public Fragment[] fragmenten;
        public string titel;
        public int aantal;
        public int index;

        public Blok(int id, Playbackband parent, string titel, int aantal, int index)
        {
            this.id = id;
            this.parent = parent;
            this.titel = titel;
            this.aantal = aantal;
            this.index = index;
            fragmenten = new Fragment[aantal];
        }

        public Blok(Playbackband parent, string titel, int aantal, int index)
        {
            this.parent = parent;
            this.titel = titel;
            this.aantal = aantal;
            this.index = index;

            fragmenten = new Fragment[aantal];
        }

        public void SetAantal(int aantal)
        {
            this.aantal = aantal;
            this.fragmenten = new Fragment[aantal];
        }

        public Fragment AddFragment(Fragment fragment)
        {
            for (int i = 0; i < fragmenten.Length; i++)
            {
                if (fragmenten[i] == null)
                {
                    fragmenten[i] = fragment;
                    return fragment;
                }
            }

            return fragment;
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() == typeof(Blok))
            {
                Blok b = (Blok)obj;
                return this.index - b.index;
            }
            else
                return 0;
        }
    }
}