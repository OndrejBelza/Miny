using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miny
{
    using FileHelpers;
    [DelimitedRecord(",")]
    public class Save
    {
        public int ID;
        public int PocetPoli;
        public int PocetMin;
        public int Objeveno;
        public float Ratio;
    }
}
