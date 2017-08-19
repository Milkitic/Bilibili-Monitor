using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Monitor
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            RegBrowser();
            RunWindow();
        }

        [STAThread]
        static void RunWindow()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        static void RegBrowser()
        {
            const string BROWSER_EMULATION_KEY = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";

            String appname = Process.GetCurrentProcess().ProcessName + ".exe";

            const int browserEmulationMode = 11001; // 11001好像是IE11

            RegistryKey browserEmulationKey =
                Registry.CurrentUser.OpenSubKey(BROWSER_EMULATION_KEY, RegistryKeyPermissionCheck.ReadWriteSubTree) ??
                Registry.CurrentUser.CreateSubKey(BROWSER_EMULATION_KEY);

            if (browserEmulationKey != null)
            {
                browserEmulationKey.SetValue(appname, browserEmulationMode, RegistryValueKind.DWord);
                browserEmulationKey.Close();
            }
        }
    }
}
