using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Monitor.Object
{
    class OBJ_Sess
    {
        public string Cookie { get; set; }
        public string Name { get; set; }
        public int Cd { get; set; }
        public CookieContainer CookieContainer;
        Random rnd = new Random();

        public OBJ_Sess(string name, string cookie)
        {
            Name = name;
            Cookie = cookie;
            CookieContainer = new CookieContainer();
        }
        public void Recover(int cd)
        {
            Cd = cd;
        }
        public void Tick()
        {
            if (Cd > 0) Cd--;
        }
    }
}
