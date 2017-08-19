using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json.Linq;

using Monitor.Object;
using Monitor.Operator;

namespace Monitor
{
    public partial class Form1 : Form
    {
        bool timeout = false;

        TempletBitmap tb;
        OriginBitmap bit;
        List<OBJ_Pixel> fixList;
        List<Rectangle> fixList_Rec;
        double progress;
        Random rnd = new Random();
        bool ifDraw = true;

        Thread t1;
        Thread t2;
        int time;
        int timeLimit = 2;

        int times = 0;

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        // Function

        private void KillT1()
        {
            t1.Abort();
        }
        private void ScanDifference()
        {
            try
            {
                fixList = new List<OBJ_Pixel>();
                fixList_Rec = new List<Rectangle>();
                try
                {
                    timeout = true;
                    tsMessage.Text = "获取数据...";
                    var str = Json.GetBitmap(Json.GetJSON());
                    timeout = false;
                    if (str == null) return;
                    bit = new OriginBitmap();
                    tsMessage.Text = "转换...";
                    if (bit.TryParse(str) == false) return;
                    tsMessage.Text = "刷新图像...";
                    Redraw();
                    tsMessage.Text = "";

                    tsProgressLabel.Text = string.Format("{0:P}", progress);
                    tsProgressBar.Value = (int)(progress * 10000);

                    Detect(fixList);

                }
                catch (ThreadAbortException)
                {
                    tsMessage.Text = string.Format("超时{0}秒，正在重新连接", timer1.Interval / 1000 * timeLimit);
                }
                catch (Exception ex)
                {
                    tsMessage.Text = ex.Message;
                }

                if (times == 0)
                {
                    Height = pictureBox1.Top + pictureBox1.Height + 75;
                    Width = pictureBox1.Width + 27 + 256;
                    splitContainer1.SplitterDistance = (Settings.x_Right - Settings.x_Left) * Settings.Scale + 16;
                    Top = Screen.PrimaryScreen.Bounds.Height / 2 - Height / 2;
                    Left = Screen.PrimaryScreen.Bounds.Width / 2 - Width / 2;
                    times++;
                }
                if (t1 != null) t1.Abort();
            }
            catch { }
        }

        private void Fix(List<OBJ_Pixel> dp)
        {
            if (Poster.MyAccount == null)
            {
                tsMsg.Text = string.Format("你还未登录，请登录。");
                toolStripMenuItem5_Click(null, null);
                return;
            }

            var MyAccount = Poster.MyAccount;
            var progressLimit = 0.9;

            int[] o_num = new int[dp.Count];
            for (int i = 0; i < o_num.Length; i++)
            {
                o_num[i] = i;
            }
            o_num = (from c in o_num orderby Guid.NewGuid() select c).ToArray();

            if (progress < progressLimit)
            {
                tsMsg.Text = "由于范围过大，不能使用辅助修复。(> 90%)";
                return;
            }
            else
            {
                int x = 0;

                if (dp.Count == 0)
                {
                    tsMsg.Text = "请稍后。";
                    return;
                }
                int i = dp[o_num[x]].I;
                int j = dp[o_num[x]].J;
                var px = tb.Pixel[i, j];
                if (OriginBitmap.TryParse(px, out string type) == false)
                {
                    tsMsg.Text = string.Format("模板好像有问题，请检查({0},{1})的颜色。", i, j);
                    return;
                }

                string[] cookieStrs = MyAccount.Cookie.ToString().Split(';');

                foreach (string str in cookieStrs)
                {
                    string[] cookieNameValue = str.Split('=');
                    Cookie ck = new Cookie(cookieNameValue[0].Trim().ToString(), cookieNameValue[1].Trim().ToString());
                    ck.Domain = "api.live.bilibili.com";
                    MyAccount.CookieContainer.Add(ck);
                }
                var pr = new Poster();
                bool isClear = true;

                if (MyAccount.Cd <= 0)
                {
                    string data = pr.SendDataByPost("https://api.live.bilibili.com/activity/v1/SummerDraw/draw", string.Format("x_min={0}&y_min={1}&x_max={0}&y_max={1}&color={2}",
                        Settings.x_Left - 1 + i, Settings.y_Up - 1 + j, type), ref MyAccount.CookieContainer);
                    JObject jo = JObject.Parse(data);
                    if (jo["code"].ToString() == "0")
                    {
                        MyAccount.Recover(int.Parse(jo["data"]["time"].ToString()));
                        tsMsg.Text = string.Format("修复了({0},{1})。", i, j);
                        isClear = false;
                        return;
                    }
                    else if (jo["code"].ToString().Equals("-101"))
                    {
                        tsMsg.Text = string.Format("登录超时了，请重新登录 {0}。", MyAccount.Name);
                        toolStripMenuItem5_Click(null, null);
                    }
                    else if (jo["code"].ToString().Equals("-400"))
                    {
                        MyAccount.Recover(int.Parse(jo["data"]["time"].ToString()));
                        tsMsg.Text = string.Format("你的账号 {0} 仍还有{1}秒的等待时间。", MyAccount.Name, MyAccount.Cd);
                    }
                    else
                    {
                        tsMsg.Text = string.Format("{0}: {1} ", MyAccount.Name, jo["message"]);
                    }
                }
                else if (isClear == true)
                {
                    tsMsg.Text = string.Format("你的账号 {0} 仍还有{1}秒的等待时间。", MyAccount.Name, MyAccount.Cd);
                }

            }
        }

        private void Detect(List<OBJ_Pixel> dp)
        {
            StringBuilder info = new StringBuilder();
            if (dp.Count == 0)
                info.Append("\r\n\r\n恭喜，图像完整。");
            else
            {
                info.Append(string.Format("\r\n\r\n发现了{0}{1}个不正确的像素", dp.Count > 1 ? "一共" : "仅", dp.Count));
                if (dp.Count > 10)
                    info.AppendLine("，请加油修复。");
                else
                {
                    info.AppendLine("：");
                    foreach (var o in dp)
                    {
                        info.AppendLine(string.Format("({0},{1})", o.I, o.J));
                    }
                }
            }
            label1.Text = info.ToString();
        }

        private void DrawTemplet()
        {
            int height = Settings.y_Down - Settings.y_Up + 1;
            int width = Settings.x_Right - Settings.x_Left + 1;
            Bitmap bmp = new Bitmap(width * Settings.Scale, height * Settings.Scale);
            Graphics gra = Graphics.FromImage(bmp);
            int a = 0;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    var rec = new Rectangle(i * Settings.Scale, j * Settings.Scale, Settings.Scale, Settings.Scale);
                    Region region1 = new Region(rec);
                    SolidBrush brush = new SolidBrush(tb.Pixel[i, j]);
                    gra.FillRegion(brush, region1);

                    if (Settings.Scale > 5)
                    {
                        Pen p = new Pen(Color.FromArgb(Settings.gAlpha, Settings.gRed, Settings.gGreen, Settings.gBlue));
                        p.Width = Settings.gWidth;
                        gra.DrawRectangle(p, rec);
                    }

                }
                a++;
            }

            for (int i = 0; i < fixList.Count; i++)
            {
                Pen pp = new Pen(Color.FromArgb(Settings.eAlpha, Settings.eRed, Settings.eGreen, Settings.eBlue));
                if (Settings.eWidth == 0)
                    pp.Width = (int)(fixList_Rec[i].Width * 0.15);
                else
                    pp.Width = Settings.eWidth;
                gra.DrawRectangle(pp, fixList_Rec[i]);
                gra.DrawLine(pp, fixList_Rec[i].Left, fixList_Rec[i].Top, fixList_Rec[i].Left + fixList_Rec[i].Width, fixList_Rec[i].Top + fixList_Rec[i].Height);
                gra.DrawLine(pp, fixList_Rec[i].Left + fixList_Rec[i].Width, fixList_Rec[i].Top, fixList_Rec[i].Left, fixList_Rec[i].Top + fixList_Rec[i].Height);
            }

            gra.Dispose();

            pictureBox2.Image = bmp;

        }

        private void Redraw()
        {
            if (bit != null && fixList != null && fixList_Rec != null)
            {
                int progressCount = 0;
                int height = Settings.y_Down - Settings.y_Up + 1;
                int width = Settings.x_Right - Settings.x_Left + 1;

                Bitmap bmp = new Bitmap(width * Settings.Scale, height * Settings.Scale);
                Graphics gra = Graphics.FromImage(bmp);
                int a = 0;
                for (int j = 0; j < height - 3; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        var rec = new Rectangle(i * Settings.Scale, j * Settings.Scale, Settings.Scale, Settings.Scale);
                        Region region1 = new Region(rec);

                        if (ifDraw == true || times < 1)
                        {
                            SolidBrush brush = new SolidBrush(bit.Pixel[i, j]);
                            gra.FillRegion(brush, region1);
                        }
                        if (Settings.CompareMode == 1 && tb.Pixel[i, j].A != 0 && tb.Pixel[i, j] != bit.Pixel[i, j])
                        {
                            fixList.Add(new OBJ_Pixel(j, i));
                            fixList_Rec.Add(rec);
                        }

                        if (Settings.CompareMode == 1 && tb.Pixel[i, j].A != 0)
                            progressCount++;
                        else if (ifDraw == true || times < 1)
                            gra.FillRegion(new SolidBrush(Color.FromArgb(Settings.cAlpha, Settings.cRed, Settings.cGreen, Settings.cBlue)), region1);

                        if ((ifDraw == true || times < 1) && Settings.Scale > 5)
                        {
                            Pen p = new Pen(Color.FromArgb(Settings.gAlpha, Settings.gRed, Settings.gGreen, Settings.gBlue));
                            p.Width = Settings.gWidth;
                            gra.DrawRectangle(p, rec);
                        }

                    }
                    a++;
                }

                if (ifDraw == true || times < 1)
                    for (int i = 0; i < fixList.Count; i++)
                    {
                        Pen pp = new Pen(Color.FromArgb(Settings.eAlpha, Settings.eRed, Settings.eGreen, Settings.eBlue));
                        if (Settings.eWidth == 0)
                            pp.Width = (int)(fixList_Rec[i].Width * 0.15);
                        else
                            pp.Width = Settings.eWidth;
                        gra.DrawRectangle(pp, fixList_Rec[i]);
                        gra.DrawLine(pp, fixList_Rec[i].Left, fixList_Rec[i].Top, fixList_Rec[i].Left + fixList_Rec[i].Width, fixList_Rec[i].Top + fixList_Rec[i].Height);
                        gra.DrawLine(pp, fixList_Rec[i].Left + fixList_Rec[i].Width, fixList_Rec[i].Top, fixList_Rec[i].Left, fixList_Rec[i].Top + fixList_Rec[i].Height);
                    }

                progress = 1 - (fixList.Count / (double)progressCount);

                gra.Dispose();

                if (ifDraw == true || times < 1) pictureBox1.Image = bmp;
            }
        }

        // Event

        private void Form1_Load(object sender, EventArgs e)
        {
            Settings.LoadSettings();
            Text = Settings.Title + " - Monitor by yf_extension";

            if (Settings.CompareMode == 1)
            {
                tb = new TempletBitmap();
                tb.ReadFromFile(@"templet.png");
                Console.WriteLine("Found compare mode opened. Loaded templet image.");
            }

            toolStripMenuItem4.Checked = ifDraw;
            Width = (Settings.x_Right - Settings.x_Left) * Settings.Scale + 16 + 256;
            splitContainer1.SplitterDistance = (Settings.x_Right - Settings.x_Left) * Settings.Scale + 16;
            Height = (Settings.y_Down - Settings.y_Up) * Settings.Scale + 40;
            Console.WriteLine("Start monitoring.");

            Top = Screen.PrimaryScreen.Bounds.Height / 2 - Height / 2;
            Left = Screen.PrimaryScreen.Bounds.Width / 2 - Width / 2;

            t1 = new Thread(ScanDifference);
            t1.Start();

            if (Poster.MyAccount == null)
            {
                var flag = MessageBox.Show("你还没有设置自己的账号用于协助修复，现在设置？", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (flag == DialogResult.Yes)
                    toolStripMenuItem5_Click(sender, e);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeout == true)
            {
                time++;
                if (time == timeLimit)
                {
                    time = 0;
                    t2 = new Thread(KillT1);
                    t2.Start();
                    timeout = false;
                    Function.ConsoleWriteLine(String: string.Format("Timed out for {0}s. Retry.", timer1.Interval / 1000 * timeLimit),
                             ForeColor: ConsoleColor.DarkGray);
                }
            }

            if (times == 1)
            {
                times++;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            if (times >= 1)
            {
                int scale;
                if (pictureBox1.Height < pictureBox1.Width)
                    scale = (int)((pictureBox1.Height) / (float)(Settings.y_Down - Settings.y_Up + 1)) + 1;
                else
                    scale = (int)((pictureBox1.Width) / (float)(Settings.x_Right - Settings.x_Left + 1)) + 1;
                Settings.Scale = scale;
            }
            if (t1 != null && t1.ThreadState == ThreadState.Aborted || t1.ThreadState == ThreadState.AbortRequested || t1.ThreadState == ThreadState.Stopped)
            {
                t1 = new Thread(ScanDifference);
                t1.Start();
            }
            else
            {
                //Functions.ConsoleWriteLine(String: "Waiting for the previous request...",
                //    ForeColor: ConsoleColor.DarkGray);
            }
        }

        private void 总在最前ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            总在最前ToolStripMenuItem.Checked = !总在最前ToolStripMenuItem.Checked;
            TopMost = 总在最前ToolStripMenuItem.Checked;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (Poster.MyAccount != null) Poster.MyAccount.Tick();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            t1.Abort();
            if (t1 != null && (t1.ThreadState == ThreadState.Aborted || t1.ThreadState == ThreadState.AbortRequested))
            {
                Json.MyClient.Dispose();
            }
        }

        private void 关于AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://milkitic.cn/Monitor");
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            ////Thread t = new Thread(Redraw);
            //t.Start();
            tsProgressBar.Width = (int)(Width * 0.4);
            button1.Left = splitContainer1.Panel2.Width - button1.Width - 5;
            button1.Top = splitContainer1.Panel2.Height - button1.Height - 32 - 5;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("config.ini");
        }

        private void osuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://osu.ppy.sh/home");
        }

        private void 启用原图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            启用原图ToolStripMenuItem.Checked = !启用原图ToolStripMenuItem.Checked;
            pictureBox2.Visible = 启用原图ToolStripMenuItem.Checked;
            if (pictureBox2.Visible == true) DrawTemplet();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            toolStripMenuItem4.Checked = !toolStripMenuItem4.Checked;
            ifDraw = toolStripMenuItem4.Checked;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            FrmLogin fl = new FrmLogin();
            fl.ShowDialog();
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            button1.Left = splitContainer1.Panel2.Width - button1.Width - 5;
            button1.Top = splitContainer1.Panel2.Height - button1.Height - 32 - 5;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Fix(fixList);
        }
    }
}
