using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

using Monitor.Object;

namespace Monitor.Operator
{
    class Json
    {
        public static WebClient MyClient { get; set; }
        public static string GetJSON()
        {
            MyClient = new WebClient();
            StreamReader MyReader = null;
            string MyWebCode;
            try
            {
                var stream = MyClient.OpenRead(@"https://api.live.bilibili.com/activity/v1/SummerDraw/bitmap");
                MyReader = new StreamReader(stream);
                MyWebCode = MyReader.ReadToEnd();
                MyWebCode = Legal(MyWebCode);
            }
            catch
            {
                MyWebCode = "";
            }
            MyReader.Close();
            return MyWebCode;
        }

        public static string GetBitmap(string json)
        {
            try
            {
                OBJ_BitmapCode Bitmap = JsonConvert.DeserializeObject<OBJ_BitmapCode>(json);
                return Bitmap.bitmap;
            }
            catch
            {
                return null;
            }
        }

        private static string Legal(string json)
        {
            json = json.Replace("\"data\":{", "");
            json = json.Replace("}}", "}");
            return json;
        }
    }
}
