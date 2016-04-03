using System;
using System.Collections.Generic;
using System.Text;

namespace pbbcore.pbb
{
    class Blok
    {
        private List<Fragment> fragmenten;
        private string name;

        public Blok(string name)
        {
            this.name = name;
            this.fragmenten = new List<Fragment>();
        }

        
    }
}
