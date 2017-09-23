using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IQBConsole
{
    public partial class WebForm : Form
    {
        public WebForm()
        {
            InitializeComponent();
        }

        private void bn_close_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void bn_start_Click(object sender, EventArgs e)
        {
            
            string url = "http://www.kakacaifu.com/Wap/idai/loanCenter.html";
            //url = "http://www.baidu.com";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = @"Mozilla/5.0 (iPhone; CPU iPhone OS 8_0 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Mobile/12A365 MicroMessenger/5.4.1 NetType/WIF";
            HttpWebResponse myRes = (HttpWebResponse)request.GetResponse();
            Stream resStream = myRes.GetResponseStream();
            StreamReader sr = new StreamReader(resStream);
            string resStr = sr.ReadToEnd();
            //    .Url = new Uri(url);
             this.webBrowser1.DocumentText = resStr;


        }
    }
}
