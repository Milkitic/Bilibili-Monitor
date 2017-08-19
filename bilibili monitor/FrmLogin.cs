using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Monitor.Operator;

namespace Monitor
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            InitializeComponent();
            // 
            // webBrowser1
            // 
            this.Controls.Add(this.webBrowser1);
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.AllowNavigation = true;
            this.webBrowser1.ScriptErrorsSuppressed = false;
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(284, 261);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
        }

        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd);

        public System.Windows.Forms.WebBrowser webBrowser1;


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            ShellExecute(IntPtr.Zero, "open", "rundll32.exe", " InetCpl.cpl,ClearMyTracksByProcess 2", "", ShowCommands.SW_HIDE);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }

        // getcookie
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int InternetSetCookieEx(string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        // wb
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        [DllImport("KERNEL32.DLL", EntryPoint = "GetCurrentProcess", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        internal static extern IntPtr GetCurrentProcess();

        private void button2_Click(object sender, EventArgs e)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            CookieContainer cookieContainer = new CookieContainer();

            if (!InternetGetCookieEx(webBrowser1.Url.ToString(), null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return;

                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(webBrowser1.Url.ToString(), null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return;
            }
            var html = webBrowser1.Document.Body.InnerHtml;
            var string1 = "<div class=\"uname\"><b>";
            int a = html.IndexOf(string1);
            int b = html.IndexOf("</b>", a);
            string uname = null;
            if (a != -1 && b != -1)
                uname = html.Substring(a + string1.Length, b - a - string1.Length);

            MyCookieOperator.SetMyCookie(uname, cookieData.ToString());
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Url == new Uri("https://www.bilibili.com/"))
            {
                button2_Click(sender, e);
                Visible = false;
                MyCookieOperator.UpdateMe();
                webBrowser1.Dispose();
                webBrowser2.Dispose();
                webBrowser3.Dispose();
                IntPtr pHandle = GetCurrentProcess();
                SetProcessWorkingSetSize(pHandle, -1, -1);
                GC.Collect();
                return;
            }
            string ok = webBrowser1.Document.Body.InnerHtml;
            System.Diagnostics.Debug.WriteLine(ok);
            if (ok == null) return;
            int a = ok.IndexOf("<img style=\"display: block;\" src=\"data:image/png");
            if (a == -1) a = ok.IndexOf("<img src=\"data:image/png");
            if (a == -1)
            {
                WordLogin();
                return;
            }
            int b = ok.IndexOf(">", a);
            string p = ok.Substring(a, b - a + 1);
            webBrowser2.Document.Body.InnerHtml = "<style>body{text-align:center; font-family:\"微软雅黑\";}.label{margin-top:5px;}.lb{margin-top:1px;color:rgb(128,128,128);font-size:10pt;}.centre{padding:5px;text-align:center;position:absolute;left:50%;margin-left:-104px;width:200px;top:50%;margin-top:-97px;height:186px;box-shadow:RGBA(0,0,0,0.2) 0px 0px 10px;overflow:hidden;}.centre img{left:50%;margin-left:-70px;position:relative}a{color:#428bca;text-decoration:none;}a:hover{color:#0094ff;}a:active{color:#0094ff;}a:visited{color:#428bca;}</style><div class=\"centre\">" + p + "<div class=\"label\">推荐使用二维码扫描</div><div class=\"lb\"><a href=\"https://www.milkitic.cn/banner/success.html\">账号密码登录</a></div></div>";
        }

        private void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var a = webBrowser2.Document.Body.InnerHtml;
            var b = "<div style=\"display: none;\">success</div>";
            if (a != null)
                if (a.Trim('\n').Trim() == b)
                {
                    WordLogin();
                }
        }

        private void webBrowser3_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser1.Url = new System.Uri("https://passport.bilibili.com/login", System.UriKind.Absolute);
        }
        private void WordLogin()
        {
            webBrowser2.Visible = false;
            this.Width = 1280;
            this.Height = 720;
            Left = Screen.PrimaryScreen.Bounds.Width / 2 - Width / 2;
            Top = Screen.PrimaryScreen.Bounds.Height / 2 - Height / 2;
        }
    }
}
