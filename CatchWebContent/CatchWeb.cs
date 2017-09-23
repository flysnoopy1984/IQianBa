using HtmlAgilityPack;
using IQBWX.DataBase;
using IQBWX.Models.Crawler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace CatchWebContent
{
    public class CatchWeb
    {
        private bool _IsFromIQB;
        private Encoding _EnCode;
        private RichTextBox _Control;
        private List<string> _FilterUrls;
        public CatchWeb(bool isfromIQB = false,RichTextBox control = null)
        {
            _IsFromIQB = isfromIQB;
            _Control = control;
            _EnCode = Encoding.UTF8;

            _FilterUrls = new List<string>();
            _FilterUrls.Add("/m/article/400");
            _FilterUrls.Add("/m/article/376");
            _FilterUrls.Add("/m/article/375");
        }

        //origianl URL http://hjc025.xiaoyun.com/m/;
        public string GetHtmlStr(string url, Encoding encoding )
        {
            try
            {
                WebRequest rGet = WebRequest.Create(url);
                WebResponse rSet = rGet.GetResponse();
                Stream s = rSet.GetResponseStream();
                StreamReader reader = new StreamReader(s, encoding);
                return reader.ReadToEnd();
            }
            catch (WebException)
            {
                //连接失败
                return null;
            }
        }

        private void WriteOut(string text)
        {
            if (_Control != null)
                _Control.AppendText(text+"\n");
        }

        public string FormatPublishDate(string date)
        {
            switch(date)
            {
                case "1天前":
                    return DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                case "2天前":
                    return DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");
                case "3天前":
                    return DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
                default:
                    if (date.IndexOf("小时") > 0)
                    {
                        return DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                        return date;
            }
        }

        public bool IsFilterUrls(string url)
        {
            return (_FilterUrls.IndexOf(url) > -1);
            
        }
            

        public EInfoDetail AnalyDetail(string url, EInfoSummery summery)
        {
            string htmlstr = GetHtmlStr(url, Encoding.UTF8);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(htmlstr);
           
            HtmlNode rootnode = doc.DocumentNode;

            string xpathstring = "//td[@id='article_content']";
            HtmlNode node = rootnode.SelectSingleNode(xpathstring);

            node.ChildNodes[0].Remove();
           // node.ChildNodes[0].Remove();

            EInfoDetail ed = new EInfoDetail()
            {
                OrigUrl = url,
                ArticleContent = node.OuterHtml,
                EInfoSummeryId = summery.InfoId,
                Title = summery.Title,
            };
            
          
        

            return ed;
        }


        public void run(string url,Encoding code)
        {
            _EnCode = code;
            Dictionary<string, EInfoSummery> detailList = new Dictionary<string, EInfoSummery>();
            try
            { 
                string htmlstr = GetHtmlStr(url, code);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(htmlstr);
                HtmlNode rootnode = doc.DocumentNode;
                string xpathstring = "";
                if (!_IsFromIQB)
                {
                    xpathstring = "//a[text() ='最新技术']";
                    HtmlNodeCollection listAddr = rootnode.SelectNodes(xpathstring);
                    url += listAddr[0].Attributes["href"].Value;
                    htmlstr = GetHtmlStr(url, Encoding.UTF8);
                    doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(htmlstr);
                    rootnode = doc.DocumentNode;
                }

                xpathstring = "//span[@class='list-title-size']";
                HtmlNodeCollection titles = rootnode.SelectNodes(xpathstring);    //所有找到的节点都是一个集合
              
                xpathstring = "//div[@class='cell-flat-content list-content-size']";
                HtmlNodeCollection ess = rootnode.SelectNodes(xpathstring);
              
                xpathstring = "//div[@class='image-container undefined ']";
                HtmlNodeCollection img = rootnode.SelectNodes(xpathstring);

                xpathstring = "//div[@class='cell-flat-time']";
                HtmlNodeCollection date = rootnode.SelectNodes(xpathstring);

                xpathstring = "//div[@class='cell-flat-eye']";
                HtmlNodeCollection rc = rootnode.SelectNodes(xpathstring);

                xpathstring = "//a[@class='cell-flat']";
                HtmlNodeCollection detail = rootnode.SelectNodes(xpathstring);

           
                using (Html5Content db = new Html5Content())
                {
                    for (int i = 0; i < titles.Count(); i++)
                    {
                        string detailUrl = "http://hjc025.xiaoyun.com" + detail[i].Attributes["href"].Value;
                        if(this.IsFilterUrls(detail[i].Attributes["href"].Value))
                        {
                            WriteOut("Url Filted -- " + detailUrl);
                            continue;
                        }
                        if (!db.IsExistSummery(detail[i].Attributes["href"].Value))
                        {
                            EInfoSummery es = new EInfoSummery();
                            es.ReadCount = Convert.ToInt32(rc[i].InnerText.Replace("阅读", ""));
                            es.Title = titles[i].InnerText;
                            es.Summery = ess[i].InnerText;
                            es.PublishDate = this.FormatPublishDate(date[i].InnerText);
                            if (!_IsFromIQB)
                                es.CoverImg = img[i + 7].Attributes["src"].Value;
                            else
                                es.CoverImg = img[i + 9].Attributes["src"].Value;

                            es.OrigInfoId = detail[i].Attributes["href"].Value;
                            es.CreateDateTime = DateTime.Now;
                            db.InfoSummery.Add(es);

                           
                            //detailList.Add(detailUrl, es);

                            using (TransactionScope sc = new TransactionScope())
                            {
                                db.SaveChanges();
                                EInfoDetail ed = this.AnalyDetail(detailUrl, es);
                                db.InfoDetail.Add(ed);
                                db.SaveChanges();
                                sc.Complete();
                            }
                            WriteOut("Analy Url -- " + detailUrl + " Done");
                         
                        }
                        else
                        {
                            WriteOut("Url Existed -- " + detailUrl);
                            continue;
                           
                        }
                        //EInfoDetail ed = this.AnalyDetail(detailUrl, es);
                        //db.InfoDetail.Add(ed);
                    }

                 

                  //  this.AnalyDetail(detailList);
                    
            }      



                    //}
                    //sc.Complete();
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }
    }
}
