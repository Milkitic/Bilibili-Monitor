using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Monitor
{
    class TempletBitmap
    {
        private Color[,] pixel;
        // 从配置中获取大小 [弃用]
        // private Color[,] pixel = new Color[Settings.tWidth, Settings.tHeight];
        public Color[,] Pixel { get => pixel; set => pixel = value; }
        public Image Result { get; set; }
        public void ReadFromFile(string root)
        {
            FileStream fs = File.OpenRead(root); //OpenRead
            Image img = Image.FromStream(fs);
            int width = Settings.x_Right - Settings.x_Left + 1, height = Settings.y_Down - Settings.y_Up + 1;
            //pixel = new Color[img.Width, img.Height]; //获取长宽定义大小

            pixel = new Color[img.Width > width ? img.Width : width,
                img.Height > height ? img.Height : height]; //获取长宽定义大小

            int filelength = 0;
            filelength = (int)fs.Length; // 获得文件长度 
            Byte[] image = new Byte[filelength]; // 建立一个字节数组 
            fs.Read(image, 0, filelength); // 按字节流读取 

            Result = Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(Result);
            for (int j = 0; j < bit.Height; j++)
            {
                for (int i = 0; i < bit.Width; i++)
                {
                    pixel[i, j] = bit.GetPixel(i, j);
                }
            }
        }

    }
}
