using IQBWX.BLL;
using IQBWX.BLL.NT;
using IQBWX.DataBase;
using IQBWX.Models.Order;
using IQBWX.Models.Results;
using IQBWX.Models.Transcation;
using IQBWX.Models.User;
using IQBWX.Models.WX.Template;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ConsoleApplication1
{
    public class Demo
    {


        public static string GetRnd(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;

            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }

            return s;
        }

        public static void DBinit()
        {
            using (UserContent db = new UserContent())
            {
                db.Get("111");
                db.GetMemberInfoByOpenId("111");
            }
        }

        public static void test()
        {
            EOrderLine order = null;
            EMemberInfo mi = null;
            using (TransactionScope sc = new TransactionScope())
            {
                using (UserContent db = new UserContent())
                {
                    string openId = "orKUAw16WK0BmflDLiBYsR-Kh5bE";
             
                    db.UpdateToMemberLevel2(openId);
                    mi = db.GetMemberInfoByOpenId(openId);
                    db.SaveChanges();
                }
                using (OrderContent odb = new OrderContent())
                {
                   
                    order = odb.UpdateToPaidOrder(mi);

                }
            }
        }


       public static void DbTest()
        {
            
            IQBWX.DataBase.UserContent db = new IQBWX.DataBase.UserContent();
            //DbRawSqlQuery<int> r = db.Database.SqlQuery<int>("select userId from MemberInfo m ");
            //foreach (int item in r)
            //{
            //    Console.WriteLine(item);
            //}
             Console.WriteLine(db.GetParentOpenId("test"));



        }

        public static string sign()
        {
            string KEY = "y58ohva8wsmw6dtshg5ccvkp9khan39g";
            string stringA = "act_name=全民秒贷提款红包&client_ip=121.43.165.5&mch_billno=1440325302201703221490192778&mch_id=1440325302&nonce_str=xfnoTmhjfme9d3uEAhzuC0hznJw8LU&re_openid=orKUAw16WK0BmflDLiBYsR-Kh5bE&send_name=全民秒贷&total_amount=100&total_num=1&wishing=提款秒到账&wxappid=wx041fa0b86badb080";
            string SignTemp = stringA + "&" + "key=" + KEY;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(SignTemp))).ToUpper().Replace("-", "");
            return sign;
        }

        public static void TemplateTest()
        {
           

            using (UserContent db = new UserContent())
            {
                EUserInfo ui = db.Get("11");
                int i = 1;
            }
    
           //string template =JsonConvert.SerializeObject(data);
        }

        //public static string SMSTest()
        //{
        //    SMS sms = new SMS();
        //    return sms.TestSMS();
        //}

        public static List<RARTrans> PageTest(int pageIndex)
        {
            List < RARTrans > result= null;

            UserContent udb = new UserContent();
            var ml = udb.GetMemberInfoByOpenId("999");
                 
                 
            TransContent db = new TransContent();
                   

                var list = db.ARTransDbSet.Where(t => t.openId == "999").Join(db.MemberInfo, a => a.FromOpenId, b => b.openId,
                    (a, b) => new RARTrans
                    {
                        ChildNickName = b.nickname,
                        openId = a.openId,
                        Amount = a.Amount,
                        TransDateTime = a.TransDateTime,
                        TransId = a.TransId,
                        TransRemark = a.TransRemark
                    });

            int count = list.Count();
                list = list.OrderBy(t => t.TransDateTime);
                result = list.Skip(pageIndex * 10).Take(10).ToList();
             
               
            
            return result;
           
        }
    }
}
