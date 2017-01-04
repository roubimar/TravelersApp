using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeTravelersApp.DataClass
{
    public class CommandButton
    {
        public string Icon { get; set; }
        public string Capture { get; set; }
        public Action Command { get; set; }
    }
}
