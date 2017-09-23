using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;

namespace IQBWX.Common
{
    public class ImgHelper
    {
        public static string BKPath = ConfigurationManager.AppSettings["QRPathPrefix"];//@"\IQB\Content\QRImg\bk_";

        //图片水印处理方法
        public static Bitmap ImageWatermark(Bitmap MainImg, Image waterimg)
        {
            

            //添加水印
            Graphics g = Graphics.FromImage(MainImg);

            //获取水印位置设置
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;
            x = MainImg.Width / 2 - waterimg.Width / 2;
            y = MainImg.Height / 2-110;
            loca.Add(x);
            loca.Add(y);

          
            g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), 140, 140));
          
            return MainImg;
        }
    }
}