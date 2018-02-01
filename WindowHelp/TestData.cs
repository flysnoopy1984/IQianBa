using IQBWX.DataBase;
using IQBWX.Models.Transcation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBConsole
{
    public class TestData
    {
        public static void APTransData()
        {
            using (TransContent db = new TransContent())
            {
                EAPUserTrans trans = null;
                for (int i = 0; i < 25; i++)
                {
                    Random r = new Random();
                    trans = new EAPUserTrans();
                    trans.openId = "orKUAw16WK0BmflDLiBYsR-Kh5bE";
                    decimal amt = (decimal)(r.Next(999) * r.NextDouble());
                    trans.Amount = amt;
                    trans.TransDateTime = DateTime.Now;
                    trans.TransRemark = string.Format("获得{0}级会员{1}的佣金{2}", r.Next(3), "test", amt);
                    db.APTransDbSet.Add(trans);
                }
                db.SaveChanges();
            }
        }

        public static void ARTransData()
        {
            using (TransContent db = new TransContent())
            {
                EARUserTrans trans = null;
                for (int i = 0; i < 25; i++)
                {
                    Random r = new Random();
                    trans = new EARUserTrans();
                    trans.FromOpenId = "orKUAw16WK0BmflDLiBYsR-Kh5bE";
                    trans.openId = "orKUAw16WK0BmflDLiBYsR-Kh5bE";
                    decimal amt = (decimal)(r.Next(999) * r.NextDouble());
                    trans.Amount = amt;
                    trans.TransDateTime = DateTime.Now;
                    trans.TransRemark = string.Format("获得{0}级会员{1}的佣金{2}", r.Next(3), "test", amt);
                    db.ARTransDbSet.Add(trans);
                }
                db.SaveChanges();
            }
        }
    }
}
