using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Monitor.Object;

namespace Monitor.Operator
{
    class Poster
    {
        public static OBJ_Sess MyAccount = MyCookieOperator.GetMyCookie();
        public OBJ_Cookies Cookies { get; set; }
        public Poster() { }
        public Poster(string root)
        {
            Cookies = new OBJ_Cookies();
            string cookie = File.ReadAllText(root);
            string[] cookieList = cookie.Split(';');
            for (int i = 0; i < cookieList.Length; i++)
            {
                string[] tmp = cookieList[i].Split('=');
                Cookies.Name.Add(tmp[0].Trim());
                Cookies.Param.Add(tmp[1].Trim());
            }
        }

        public string SendDataByPost(string Url, string postDataStr, ref CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            if (cookie.Count == 0)
            {
                request.CookieContainer = new CookieContainer();
                cookie = request.CookieContainer;
            }
            else
            {
                request.CookieContainer = cookie;
            }

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}
