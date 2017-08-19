using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor
{
    class OriginBitmap
    {
        private Color[,] originPixel = new Color[1280, 720];

        public Color[,] OriginPixel { get => originPixel; set => originPixel = value; }

        private Color[,] pixel = new Color[Settings.x_Right - Settings.x_Left + 1, Settings.y_Down - Settings.y_Up + 1];
        public Color[,] Pixel { get => pixel; set => pixel = value; }

        public static Bitmap Colors { get; set; }
        public static string[,] Color_s { get; set; }

        public static void ReadColor()
        {
            Colors = new Bitmap("COLOR.png");
            Color_s = new string[Colors.Width, Colors.Height];
            int a = 0;
            char b = 'A';
            for (int j = 0; j < Colors.Height; j++)
            {
                for (int i = 0; i < Colors.Width; i++)
                {
                    if (a < 10)
                    {
                        Color_s[i, j] = a.ToString();
                        a++;
                    }
                    else
                    {
                        Color_s[i, j] = b.ToString();
                        b++;
                    }
                }
            }
        }

        public static bool TryParse(Color px, out string type)
        {
            type = null;

            for (int j = 0; j < Colors.Height; j++)
            {
                for (int i = 0; i < Colors.Width; i++)
                {
                    if (px == Colors.GetPixel(i, j))
                    {
                        type = Color_s[i, j];
                        return true;
                    }
                }
            }
            return false;
            #region
            //if (px == Color.FromArgb(0, 0, 0)) type = "0";
            //else if (px == Color.FromArgb(255, 255, 255)) type = "1";
            //else if (px == Color.FromArgb(170, 170, 170)) type = "2";
            //else if (px == Color.FromArgb(85, 85, 85)) type = "3";
            //else if (px == Color.FromArgb(254, 211, 199)) type = "4";
            //else if (px == Color.FromArgb(255, 196, 206)) type = "5";
            //else if (px == Color.FromArgb(250, 172, 142)) type = "6";
            //else if (px == Color.FromArgb(255, 139, 131)) type = "7";
            //else if (px == Color.FromArgb(244, 67, 54)) type = "8";
            //else if (px == Color.FromArgb(233, 30, 99)) type = "9";
            //else if (px == Color.FromArgb(226, 102, 158)) type = "A";
            //else if (px == Color.FromArgb(156, 39, 176)) type = "B";
            //else if (px == Color.FromArgb(103, 58, 183)) type = "C";
            //else if (px == Color.FromArgb(63, 81, 181)) type = "D";
            //else if (px == Color.FromArgb(0, 70, 112)) type = "E";
            //else if (px == Color.FromArgb(5, 113, 151)) type = "F";
            //else if (px == Color.FromArgb(33, 150, 243)) type = "G";
            //else if (px == Color.FromArgb(0, 188, 212)) type = "H";
            //else if (px == Color.FromArgb(59, 229, 219)) type = "I";
            //else if (px == Color.FromArgb(151, 253, 220)) type = "J";
            //else if (px == Color.FromArgb(22, 115, 0)) type = "K";
            //else if (px == Color.FromArgb(55, 169, 60)) type = "L";
            //else if (px == Color.FromArgb(137, 230, 66)) type = "M";
            //else if (px == Color.FromArgb(215, 255, 7)) type = "N";
            //else if (px == Color.FromArgb(255, 246, 209)) type = "O";
            //else if (px == Color.FromArgb(248, 203, 140)) type = "P";
            //else if (px == Color.FromArgb(255, 235, 59)) type = "Q";
            //else if (px == Color.FromArgb(255, 193, 7)) type = "R";
            //else if (px == Color.FromArgb(255, 152, 0)) type = "S";
            //else if (px == Color.FromArgb(255, 87, 34)) type = "T";
            //else if (px == Color.FromArgb(184, 63, 39)) type = "U";
            //else if (px == Color.FromArgb(121, 85, 72)) type = "V";
            //else
            //{
            //    type = null;
            //    return false;
            //}
            //return true;
            #endregion
        }

        public bool TryParse(string bitmapText)
        {
            if (bitmapText == null) return false;
            int a = 0;

            for (int j = 0; j < 720; j++)
            {
                for (int i = 0; i < 1280; i++)
                {
                    if (j < Settings.y_Up - 1 || j > Settings.y_Down || i < Settings.x_Left - 1 || i > Settings.x_Right) continue;
                    var text = bitmapText.Substring(i + a * 1280, 1);

                    bool flag = false;
                    for (int v = 0; v < Colors.Height; v++)
                    {
                        for (int u = 0; u < Colors.Width; u++)
                        {
                            if (text == Color_s[u, v])
                            {
                                originPixel[i, j] = Colors.GetPixel(u, v);
                                flag = true;
                                break;
                            }
                        }
                        if (flag == true) break;
                    }
                    #region
                    //switch (text)
                    //{
                    //    case "0":
                    //        originPixel[i, j] = Color.FromArgb(0, 0, 0);
                    //        break;
                    //    case "1":
                    //        originPixel[i, j] = Color.FromArgb(255, 255, 255);
                    //        break;
                    //    case "2":
                    //        originPixel[i, j] = Color.FromArgb(170, 170, 170);
                    //        break;
                    //    case "3":
                    //        originPixel[i, j] = Color.FromArgb(85, 85, 85);
                    //        break;
                    //    case "4":
                    //        originPixel[i, j] = Color.FromArgb(254, 211, 199);
                    //        break;
                    //    case "5":
                    //        originPixel[i, j] = Color.FromArgb(255, 196, 206);
                    //        break;
                    //    case "6":
                    //        originPixel[i, j] = Color.FromArgb(250, 172, 142);
                    //        break;
                    //    case "7":
                    //        originPixel[i, j] = Color.FromArgb(255, 139, 131);
                    //        break;
                    //    case "8":
                    //        originPixel[i, j] = Color.FromArgb(233, 30, 99);
                    //        break;
                    //    case "9":
                    //        originPixel[i, j] = Color.FromArgb(226, 102, 158);
                    //        break;
                    //    case "A":
                    //        originPixel[i, j] = Color.FromArgb(156, 39, 176);
                    //        break;
                    //    case "B":
                    //        originPixel[i, j] = Color.FromArgb(5, 113, 151);
                    //        break;
                    //    case "C":
                    //        originPixel[i, j] = Color.FromArgb(103, 58, 183);
                    //        break;
                    //    case "D":
                    //        originPixel[i, j] = Color.FromArgb(63, 81, 181);
                    //        break;
                    //    case "E":
                    //        originPixel[i, j] = Color.FromArgb(0, 70, 112);
                    //        break;
                    //    case "F":
                    //        originPixel[i, j] = Color.FromArgb(5, 113, 151);
                    //        break;
                    //    case "G":
                    //        originPixel[i, j] = Color.FromArgb(33, 150, 243);
                    //        break;
                    //    case "H":
                    //        originPixel[i, j] = Color.FromArgb(0, 188, 212);
                    //        break;
                    //    case "I":
                    //        originPixel[i, j] = Color.FromArgb(59, 229, 219);
                    //        break;
                    //    case "J":
                    //        originPixel[i, j] = Color.FromArgb(151, 253, 220);
                    //        break;
                    //    case "K":
                    //        originPixel[i, j] = Color.FromArgb(22, 115, 0);
                    //        break;
                    //    case "L":
                    //        originPixel[i, j] = Color.FromArgb(55, 169, 60);
                    //        break;
                    //    case "M":
                    //        originPixel[i, j] = Color.FromArgb(137, 230, 66);
                    //        break;
                    //    case "N":
                    //        originPixel[i, j] = Color.FromArgb(215, 255, 7);
                    //        break;
                    //    case "O":
                    //        originPixel[i, j] = Color.FromArgb(255, 246, 209);
                    //        break;
                    //    case "P":
                    //        originPixel[i, j] = Color.FromArgb(248, 203, 140);
                    //        break;
                    //    case "Q":
                    //        originPixel[i, j] = Color.FromArgb(255, 235, 59);
                    //        break;
                    //    case "R":
                    //        originPixel[i, j] = Color.FromArgb(255, 193, 7);
                    //        break;
                    //    case "S":
                    //        originPixel[i, j] = Color.FromArgb(255, 152, 0);
                    //        break;
                    //    case "T":
                    //        originPixel[i, j] = Color.FromArgb(255, 87, 34);
                    //        break;
                    //    case "U":
                    //        originPixel[i, j] = Color.FromArgb(184, 63, 39);
                    //        break;
                    //    case "V":
                    //        originPixel[i, j] = Color.FromArgb(121, 85, 72);
                    //        break;
                    //}
                    #endregion
                }
                a++;
            }

            a = 0;
            for (int j = Settings.y_Up; j <= Settings.y_Down; j++)
            {
                for (int i = Settings.x_Left; i <= Settings.x_Right; i++)
                {
                    int realI = i - Settings.x_Left, realJ = j - Settings.y_Up;
                    pixel[realI, realJ] = originPixel[i - 1, j - 1];
                }
                a++;
            }
            return true;
        }
    }
}
