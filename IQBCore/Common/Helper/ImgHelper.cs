using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Common.Helper
{
    public class ImgHelper
    {

        public static Bitmap CreateBlankImg(int w,int h, Brush b)
        {
            Bitmap newBK = new Bitmap(w,h);
            using (Graphics g = Graphics.FromImage(newBK))
            {
                g.FillRectangle(b, new Rectangle(0, 0, newBK.Width, newBK.Height));
            }
            return newBK;
        }
        public static Image AddImgBorder(Bitmap resultImg,int BorderWidth,Color c)
        {
            try
            {
                
                float w = BorderWidth;
               
                using (Graphics g = Graphics.FromImage(resultImg))
                {
                    using (Brush brush = new SolidBrush(c))
                    {
                        using (Pen pen = new Pen(brush, w))
                        {
                            pen.DashStyle = DashStyle.Custom;
                            g.DrawRectangle(pen, new Rectangle(0, 0, Math.Abs(resultImg.Width), Math.Abs(resultImg.Height)));
                            g.Dispose();
                         
                        }
                    }
                }
                return resultImg;
            }
            catch (Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("AddImgBorder Error:" + ex.Message);
                throw ex;
            }
        }
        public static Image GetImgFromUrl(string httpurl)
        {
            try
            {
                System.Net.WebRequest webreq = System.Net.WebRequest.Create(httpurl);
                System.Net.WebResponse webres = webreq.GetResponse();
                System.IO.Stream stream = webres.GetResponseStream();
                Image result = Image.FromStream(stream);
                return result;

            }
            catch (Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("GetImgFromUrl Error:" + ex.Message);

                throw ex;
            }

        }
        public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //获取图片宽度
            int sourceWidth = imgToResize.Width;
            //获取图片高度
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //计算宽度的缩放比例
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //计算高度的缩放比例
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //期望的宽度
            int destWidth = (int)(sourceWidth * nPercent);
            //期望的高度
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //绘制图像
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        public static string BKPath = ConfigurationManager.AppSettings["QRPathPrefix"];//@"\IQB\Content\QRImg\bk_";

        public static Bitmap CombineImage(Bitmap BKImg, Image waterimg)
        {

            //添加水印
            using (Graphics g = Graphics.FromImage(BKImg))
            {
                ArrayList loca = new ArrayList();
                int x = 0;
                int y = 0;
                x = BKImg.Width / 2 - waterimg.Width / 2;
                y = BKImg.Height / 2 - waterimg.Height / 2;

                loca.Add(x);
                loca.Add(y);


                g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Width));
                g.Dispose();
            }

                //获取水印位置设置
               

            return BKImg;
        }

        //图片水印处理方法
        public static Bitmap ImageWatermark(Bitmap MainImg, Image waterimg)
        {

            //添加水印
            using (Graphics g = Graphics.FromImage(MainImg))
            {
                //获取水印位置设置
                ArrayList loca = new ArrayList();
                int x = 0;
                int y = 0;
                x = MainImg.Width / 2 - waterimg.Width / 2;
                y = MainImg.Height / 2 - waterimg.Height / 2;
                loca.Add(x);
                loca.Add(y);


                g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Height));
            }

           
           
            return MainImg;
        }
    }
}
