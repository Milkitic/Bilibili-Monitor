using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Object
{
    class OBJ_Pixel
    {
        public int I { get; set; }
        public int J { get; set; }
        public OBJ_Pixel(int j,int i)
        {
            J = j;
            I = i;
        }
    }
}
