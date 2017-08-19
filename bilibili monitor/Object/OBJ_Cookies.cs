using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Object
{
    class OBJ_Cookies
    {
        public List<string> Name { get; set; }
        public List<string> Param { get; set; }
        public OBJ_Cookies()
        {
            Name = new List<string>();
            Param = new List<string>();
        }
    }
}
