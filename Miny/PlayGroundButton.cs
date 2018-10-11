using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Miny
{

    class PlayGroundButton : Button
    {
        
        public int Row_ { get; set; }
        public int Col_ { get; set; }
        public bool IsBomb { get; set; }
        public bool HasFalg { get; set; }
        
    }
}
