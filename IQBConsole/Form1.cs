using CatchWebContent;
using IQBWX.Common;
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
    }
}
