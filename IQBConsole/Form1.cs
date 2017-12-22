using CatchWebContent;
using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBWX.Models.OutParameter;
using IQBPay.Core;
using IQBWX.Common;
using IQBWX.Controllers;
using IQBWX.Models.WX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThoughtWorks.QRCode.Codec;
using WxPayAPI;

namespace IQBConsole
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void RunCatchWeb(int type)           
        {
            CatchWeb cw = null;
            try
            {
                string url = "";
                Encoding code = Encoding.Default;
                if (type == 1)
                { 
                    url = "http://www.iqianba.cn/iqb/catchsite/abc.html";
                    code = Encoding.Default;
                    cw = new CatchWeb(true, this.tb_Result);
                }
                else
                { 
                    url = "http://hjc025.xiaoyun.com/m/";
                    code = Encoding.UTF8;
                    cw = new CatchWeb(false, this.tb_Result);
                }

                tb_Result.Text += "开始抓爬数据：" + url+"\n";
                
                cw.run(url, code);
                tb_Result.Text += "抓爬数据完成\n";
            }
            catch(Exception ex)
            {
                tb_Result.Text = "抓爬错误：" + ex.Message+"\n";
            }
        }

        /// <summary>
        /// 爬虫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => RunCatchWeb(1));
            t.IsBackground = true;
            t.Start();

        }

        /// <summary>
        /// 创建AP测试数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                
                
                tb_Result.Text= "开始创建测试数据 APTransData";
                TestData.APTransData();
                tb_Result.AppendText("\n创建完成");
            }
            catch(Exception ex )
            {
                tb_Result.AppendText("\n出错" + ex.Message);
            }
        }

        private void bn_ARData_Click(object sender, EventArgs e)
        {
            try
            {
                tb_Result.Text = "开始创建测试数据 ARTransData";
                TestData.ARTransData();
                tb_Result.AppendText("\n创建完成");
            }
            catch (Exception ex)
            {
                tb_Result.AppendText("\n出错" + ex.Message);
            }
        }

        private void bn_catchHJB_Click(object sender, EventArgs e)
        {

            Thread t = new Thread(() => RunCatchWeb(2));
            t.IsBackground = true;
            t.Start();

        }

        private void bn_webForm_Click(object sender, EventArgs e)
        {
            WebForm form = new WebForm();
            form.ShowDialog();
        }

        private void bn_MemberList_Click(object sender, EventArgs e)
        {
            //string accessToken = JsApiPay.GetAccessToken();
            //string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}",accessToken);

            //string xml = HttpHelper.HttpGet(url);
            //tb_Result.Clear();
            //tb_Result.Text = xml;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bt = null;
            try
            {
                string enCodeString = "http://wx.iqianba.cn/PP/Pay?Id=5";
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                qrCodeEncoder.QRCodeScale = 4;
                qrCodeEncoder.QRCodeVersion = 9;
                bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
                
                Bitmap blankBK = ImgHelper.CreateBlankImg(bt.Width+20,bt.Height+20,Brushes.White);
                bt = ImgHelper.CombineImage(blankBK, bt);

                string url = "http://wx.qlogo.cn/mmopen/hzVGicX27IG18yibKNnHfBojH4SpCPGNEvyOUZE8jxOw2ZnYcHzAkm7yHk0oKoCA2zqtyib09sxDzX5GOubMfyOraSMren2GUSw/0";
                Image LogoImg = ImgHelper.GetImgFromUrl(url);
                LogoImg = ImgHelper.resizeImage(LogoImg, new Size(56, 56));
                LogoImg = ImgHelper.AddImgBorder(new Bitmap(LogoImg), 4,Color.Wheat);

                pictureBox1.Image =ImgHelper.CombineImage(bt, LogoImg);

            }
            catch (Exception ex)
            {

                throw ex;
            }


            
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Bitmap bt = null;
            try
            {
                string enCodeString = "http://wx.iqianba.cn/PP/Pay?Id=5";
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                qrCodeEncoder.QRCodeScale = 4;
                qrCodeEncoder.QRCodeVersion = 9;
                bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);

                Bitmap blankBK = ImgHelper.CreateBlankImg(bt.Width + 20, bt.Height + 20, Brushes.White);
                bt = ImgHelper.CombineImage(blankBK, bt);

                string url = "http://wx.qlogo.cn/mmopen/hzVGicX27IG18yibKNnHfBojH4SpCPGNEvyOUZE8jxOw2ZnYcHzAkm7yHk0oKoCA2zqtyib09sxDzX5GOubMfyOraSMren2GUSw/0";
                Image LogoImg = ImgHelper.GetImgFromUrl(url);
                LogoImg = ImgHelper.resizeImage(LogoImg, new Size(56, 56));
                LogoImg = ImgHelper.AddImgBorder(new Bitmap(LogoImg), 4, Color.Wheat);
                bt = ImgHelper.CombineImage(bt, LogoImg); 

                Bitmap bkImg = new Bitmap(@"C:\Project\SourceCode\IQianBa\IQBConsole\ARUserBK1.jpg");

                Bitmap finImg = ImgHelper.ImageWatermark(bkImg, bt);

                pictureBox1.Image = finImg;

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            string tokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            tokenUrl = string.Format(tokenUrl, WxPayConfig.APPID, WxPayConfig.APPSECRET);
            AccessToken token = IQBCore.Common.Helper.HttpHelper.Get<AccessToken>(tokenUrl);

           

            SSOQR ssrQR = new SSOQR();

            string  QRId = IQBConstant.WXQR_IQBPAY_PREFIX + "11";

            WXQRResult resObj = GetQR("", token.access_token, QRId, false);

            string Picurl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + resObj.ticket + "";

            Image bkImg = ImgHelper.GetImgFromUrl(Picurl);

            Bitmap logo = new Bitmap(@"C:\Project\SourceCode\IQianBa\IQBConsole\Logo_AR.png");

            Bitmap finImg = ImgHelper.ImageWatermark(new Bitmap(bkImg), logo);

            pictureBox1.Image = finImg;
        }

        private WXQRResult GetQR(String account, string access_token, string ssoToken = null, bool isTemp = true)
        {
            WXQRResult resObj = null;
            try
            {

                //获取数据的地址（微信提供）
                String url = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + access_token;

                //发送给微信服务器的数据

                String jsonStr = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\":{\"scene\": {\"scene_id\":" + account + "}}}";
                if (!string.IsNullOrEmpty(ssoToken))
                {
                    if (isTemp)
                    {
                        jsonStr = "{\"expire_seconds\": \"180\",\"action_name\": \"QR_STR_SCENE\", \"action_info\":{\"scene\": {\"scene_str\":\"" + ssoToken + "\"}}}";
                    }
                    else
                    {
                        jsonStr = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\":{\"scene\": {\"scene_str\":\"" + ssoToken + "\"}}}";
                    }
                }

                //   log.log("getQR"+jsonStr);

                //post请求得到返回数据（这里是封装过的，就是普通的java post请求）
                String response = IQBCore.Common.Helper.HttpHelper.RequestUrlSendMsg(url, IQBCore.Common.Helper.HttpHelper.HttpMethod.Post, jsonStr);

                resObj = JsonConvert.DeserializeObject<WXQRResult>(response);
            }
            catch (Exception ex)
            {
                
            }
            return resObj;
        }
    }
}
