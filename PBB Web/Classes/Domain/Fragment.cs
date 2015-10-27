using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PBB_Web.Classes.Domain
{
    public class Fragment : IComparable
    {
        public int id;
        public int index;
        public Blok parent;
        public Liedje liedje;
        public string uitvoerwijze;
        public double begintijd;
        public double eindtijd;
        public double fadein;
        public double fadeout;
        public bool fadeinbinnen;
        public bool fadeoutbinnen;
        public bool rodedraad;

        public string opkomst;
        public string afgaan;

        private List<Uitvoerder> uitvoerders;
        private List<string> verlichting;

        public Fragment(int id, int index, Blok parent, string uitvoerwijze, double begintijd, double eindtijd, double fadein, double fadeout, int rodedraad)
        {
            this.id = id;
            this.index= index;
            this.parent = parent;
            this.uitvoerwijze = uitvoerwijze;
            this.begintijd = begintijd;
            this.eindtijd = eindtijd;
            this.fadein = fadein;
            this.fadeout = fadeout;
            this.rodedraad = rodedraad == 0 ? false : true;
            this.uitvoerders = new List<Uitvoerder>();
            this.verlichting = new List<string>();

            ReadFromDatabase();
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType() != typeof(Fragment))
                return 0;

            Fragment f = (Fragment)obj;

            return this.index - f.index;
        }

        private void ReadFromDatabase()
        {
            DataTable dt = Database.DatabaseConnector.GetInstance().ExecuteQuery("select uitvoerder.id, voornaam, achternaam, Fragment_Per_Blok.Uitvoerwijze from fragment_per_blok join fragmenten_per_uitvoerder on Fragment_Per_Blok.Fragment_Id = Fragmenten_Per_Uitvoerder.Fragment_Koppeling_Id join uitvoerder on uitvoerder.id = fragmenten_per_uitvoerder.uitvoerder_id where fragment_id = " + id);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int _0 = Convert.ToInt32(dt.Rows[i].ItemArray[0]);
                string _1 = (string)(dt.Rows[i].ItemArray[1] == DBNull.Value ? "" : dt.Rows[i].ItemArray[1]);
                string _2 = (string)(dt.Rows[i].ItemArray[2] == DBNull.Value ? "" : dt.Rows[i].ItemArray[2]);
                string wijze = (string)(dt.Rows[i].ItemArray[3] == DBNull.Value ? "" : dt.Rows[i].ItemArray[3]);

                this.uitvoerders.Add(new Uitvoerder(_0, _1, _2));

                this.uitvoerwijze = wijze;
            }


            dt = Database.DatabaseConnector.GetInstance().ExecuteQuery("select beschrijving from Fragment_Per_Blok join verlichting_per_fragment on Verlichting_Per_Fragment.Fragment_Koppeling_Id = Fragment_Per_Blok.Id join verlichting on verlichting.id = Verlichting_Per_Fragment.Verlichting_Id where fragment_id = " + id);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string _0 = (string)dt.Rows[i].ItemArray[0];

                this.verlichting.Add(_0);
            }

            dt = Database.DatabaseConnector.GetInstance().ExecuteQuery("SELECT manier FROM fragment_per_blok JOIN opkomst_afgaan ON fragment_per_blok.OPKOMST_ID = opkomst_afgaan.ID where fragment_id = " + id);

            if(dt.Rows.Count > 0)
            {
                this.opkomst = (string)(dt.Rows[0].ItemArray[0] == DBNull.Value ? "" : dt.Rows[0].ItemArray[0]);
            }

            dt = Database.DatabaseConnector.GetInstance().ExecuteQuery("SELECT manier FROM fragment_per_blok JOIN opkomst_afgaan ON fragment_per_blok.AFGAAN_ID = opkomst_afgaan.ID where fragment_id = " + id);

            if (dt.Rows.Count > 0)
            {
                this.afgaan = (string)(dt.Rows[0].ItemArray[0] == DBNull.Value ? "" : dt.Rows[0].ItemArray[0]);
            }
        }

        public string GetUitvoerders()
        {
            string uitv = "";
            for (int i = 0; i < uitvoerders.Count; i++)
            {
                uitv += uitvoerders[i].ToString();
                if (uitvoerders.Count - 2 == i)
                {
                    uitv += " & ";
                }
                else if (uitvoerders.Count - 1 != i)
                {
                    uitv += ", ";
                }
            }
            return uitv;
        }

        public string GetVerlichting()
        {
            string uitv = "";
            for (int i = 0; i < verlichting.Count; i++)
            {
                uitv += verlichting[i].ToString();
                if (verlichting.Count - 1 != i)
                {
                    uitv += " / ";
                }
            }
            return uitv;
        }
    }
}