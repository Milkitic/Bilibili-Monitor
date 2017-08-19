using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace Monitor
{

    static class Settings
    {
        static string iniRoot = System.Windows.Forms.Application.StartupPath + @"\config.ini";

        public static string Title { get; set; }
        public static int x_Left { get; set; }
        public static int x_Right { get; set; }
        public static int y_Up { get; set; }
        public static int y_Down { get; set; }
        public static int Scale { get; set; }
        public static int CompareMode { get; set; }
        public static string Bitmap { get; set; }

        public static int gRed { get; set; }
        public static int gGreen { get; set; }
        public static int gBlue { get; set; }
        public static int gAlpha { get; set; }
        public static int gWidth { get; set; }

        public static int tWidth { get; set; }
        public static int tHeight { get; set; }
        public static string tBitmap { get; set; }

        public static int eRed { get; set; }
        public static int eGreen { get; set; }
        public static int eBlue { get; set; }
        public static int eAlpha { get; set; }
        public static int eWidth { get; set; }

        public static int cRed { get; set; }
        public static int cGreen { get; set; }
        public static int cBlue { get; set; }
        public static int cAlpha { get; set; }

        public static int Interval { get; set; }
        public static int Border { get; set; }
        public static int Limit { get; set; }
      
        [DllImport("kernel32", CharSet = CharSet.Ansi, EntryPoint = "GetPrivateProfileStringA", ExactSpelling = true, SetLastError = true)]
        private static extern int GetPrivateProfileString([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpApplicationName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpKeyName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpDefault, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpReturnedString, int nSize, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpFileName);
        [DllImport("kernel32", CharSet = CharSet.Ansi, EntryPoint = "WritePrivateProfileStringA", ExactSpelling = true, SetLastError = true)]
        private static extern int WritePrivateProfileString([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpApplicationName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpKeyName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpString, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpFileName);

        private static string GetINI(string Section, string AppName, string lpDefault, string FileName)
        {
            string str = null;
            str = Strings.LSet(str, 256);
            GetPrivateProfileString(ref Section, ref AppName, ref lpDefault, ref str, Strings.Len(str), ref FileName);
            return Strings.Left(str, checked(Strings.InStr(str, "\0", CompareMethod.Binary) - 1));
        }

        //private static long WriteINI(string Section, string AppName, string lpDefault, string FileName)
        //{
        //    return WritePrivateProfileString(ref Section, ref AppName, ref lpDefault, ref FileName);
        //}

        public static void LoadSettings()
        {
            Title = GetINI("General", "Title", "osu!粉饼本体", iniRoot);

            x_Left = int.Parse(GetINI("Display", "x_Left", "1", iniRoot));
            x_Right = int.Parse(GetINI("Display", "x_Right", "1280", iniRoot));
            y_Up = int.Parse(GetINI("Display", "y_Up", "1", iniRoot));
            y_Down = int.Parse(GetINI("Display", "y_Down", "720", iniRoot));
            Scale = int.Parse(GetINI("Display", "Scale", "10", iniRoot));
            CompareMode = int.Parse(GetINI("Display", "CompareMode", "0", iniRoot));

            gRed = int.Parse(GetINI("Grid", "Red", "255", iniRoot));
            gGreen = int.Parse(GetINI("Grid", "Green", "255", iniRoot));
            gBlue = int.Parse(GetINI("Grid", "Blue", "255", iniRoot));
            gAlpha = int.Parse(GetINI("Grid", "Alpha", "70", iniRoot));
            gWidth = int.Parse(GetINI("Grid", "Width", "1", iniRoot));

            tBitmap = GetINI("Templet", "Bitmap", "", iniRoot);
            tWidth = int.Parse(GetINI("Templet", "Width", "90", iniRoot));
            tHeight = int.Parse(GetINI("Templet", "Height", "90", iniRoot));

            eRed = int.Parse(GetINI("Warn", "Red", "255", iniRoot));
            eGreen = int.Parse(GetINI("Warn", "Green", "255", iniRoot));
            eBlue = int.Parse(GetINI("Warn", "Blue", "255", iniRoot));
            eAlpha = int.Parse(GetINI("Warn", "Alpha", "70", iniRoot));
            eWidth = int.Parse(GetINI("Warn", "Width", "1", iniRoot));

            cRed = int.Parse(GetINI("Cover", "Red", "255", iniRoot));
            cGreen = int.Parse(GetINI("Cover", "Green", "255", iniRoot));
            cBlue = int.Parse(GetINI("Cover", "Blue", "255", iniRoot));
            cAlpha = int.Parse(GetINI("Cover", "Alpha", "165", iniRoot));
         
            Interval = int.Parse(GetINI("Others", "Interval", "500", iniRoot));
            Border = int.Parse(GetINI("Others", "Border", "0", iniRoot));
            Limit = int.Parse(GetINI("Others", "Limit", "1", iniRoot));

            OriginBitmap.ReadColor();
        }

    }
}
