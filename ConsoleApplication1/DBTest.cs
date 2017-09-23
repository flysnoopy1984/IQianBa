using IQBWX.BLL;
using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.Models.Order;
using IQBWX.Models.Product;
using IQBWX.Models.Transcation;
using IQBWX.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ConsoleApplication1
{
    public class DBTest
    {

        public static void InitOrder()
        {
            using (TransactionScope sc = new TransactionScope())
            {
                //new Member Info
                EMemberInfo newMI = new EMemberInfo();
                using (UserContent udb = new UserContent())
                {
                    newMI.openId = "TestOP";
                    newMI.UserId = 999999;
                    newMI.ParentOpenId = "o4";
                    newMI.MemberType = IQBWX.Common.MemberType.Channel;
                    newMI.RegisterDateTime = DateTime.Now;
                    udb.MemberInfo.Add(newMI);
                    udb.SaveChanges();
                }
                //new Order Info

                EOrderLine newOrder = new EOrderLine();
                using (OrderContent db = new OrderContent())
                {
                    newOrder = db.CreateOrder(newMI, 1);
                    db.SaveChanges();
                }
                sc.Complete();
            }
        }

        public static void TestMemberSuccess(EMemberInfo newMI, EOrderLine newOrder)
        {
            EMemberInfo mi = newMI;
            EOrderLine order = newOrder;
            Dictionary<int, EMemberInfo> relations = null;
            int count = 0;
            using (TransactionScope sc = new TransactionScope())
            {
                using (UserContent db = new UserContent())
                {
                    count = db.MemberInfo.Count();
                    //当第一个会员时，总计为0。
                    count++;
                    //1. 新增会员
                   // newMI = db.UpdateToMember("NewOP", 1, count.ToString());
                    //2. .建立父子关系
                    relations = new Dictionary<int, EMemberInfo>();
                    int level = 1;
                    db.BuildMemberRelation(mi,mi, ref relations, level);
                    db.SaveChanges();
                }
                //3.更新支付订单状态
                //using (OrderContent db = new OrderContent())
                //{
                //    order = db.UpdateToPaidOrder(mi.openId);
                //}

                //5.佣金策略执行
                PopPolicy policy = new PopPolicy(order);
               // policy.RunCommession(relations);

                sc.Complete();
            }
        }

        public static void APTransData()
        {
            using (TransContent db = new TransContent())
            {
                EAPUserTrans trans = null;
                for (int i = 0; i < 100; i++)
                {
                    Random r = new Random();
                    trans = new EAPUserTrans();
                    trans.openId = "999";
                    decimal amt = (decimal)(r.Next(999) * r.NextDouble());
                    trans.Amount = amt;
                    trans.TransDateTime = DateTime.Now;
                    trans.TransRemark = string.Format("获得{0}级会员{1}的佣金{2}", r.Next(3), "test", amt);
                    db.APTransDbSet.Add(trans);
                }
                db.SaveChanges();
            }
        }

        public static void InitTestTrans()
        {
            TransContent db = new TransContent();
            EARUserTrans trans = null;
            for(int i=0;i<100;i++)
            {
                Random r = new Random();
                trans = new EARUserTrans();
                trans.openId = "999";
                trans.FromOpenId = "666";
                decimal amt = (decimal)(r.Next(999) * r.NextDouble());
                trans.Amount = amt;
                trans.TransDateTime = DateTime.Now;
                trans.TransRemark = string.Format("获得{0}级会员{1}的佣金{2}",r.Next(3),"test",amt);
                db.ARTransDbSet.Add(trans);
            }
            db.SaveChanges();
        }

        public static void TestMySQL()
        {
          

            Database.SetInitializer(new DropCreateDatabaseAlways<InitContent>());
            var c2 = new InitContent();
            c2.UserInfo.Count();
        }

        public static void InitDBTable()
        {
            using (var context = new InitContent())
            {
                context.Database.Initialize(true);
                context.ForInit();
            }
          
           
        }

        public static void AddTestMember()
        {
            using (UserContent db = new UserContent())
            {
                string pOpenId = "orKUAw16WK0BmflDLiBYsR-Kh5bE";
              
                for (int level=1;level<=4;level++)
                {                   
                    for (int i = 1; i <=20; i++)
                    {
                        EMemberInfo mi = new EMemberInfo();
                        mi.openId = "999" + "_" + level + "_" + i;
                        
                        mi.ParentOpenId = pOpenId;
                        mi.MemberType = IQBWX.Common.MemberType.Channel;
                        mi.RegisterDateTime = DateTime.Now;
                        mi.UserId = i*level;
                        mi.nickname = StringHelper.GetRnd(10, false, true, true, false, "");
                        db.MemberInfo.Add(mi);

                        EMemberChildren mc = new EMemberChildren();
                        mc.ChildLevel = level;
                        mc.cMemberType = MemberType.Channel;
                        mc.pOpenId = pOpenId;
                        mc.cOpenId = mi.openId;
                        db.MemberChildren.Add(mc);

                    }
                }
                db.SaveChanges();
            }
        }
        public static void InitMember()
        {
            using (UserContent db = new UserContent())
            { 

                EMemberInfo mi = new EMemberInfo();
                mi.openId = "999";
                mi.MemberType = IQBWX.Common.MemberType.Channel;
                mi.RegisterDateTime = DateTime.Now;
                mi.UserId = 1;
               
                db.MemberInfo.Add(mi);

                mi = new EMemberInfo();
                mi.openId = "666";
                mi.ParentOpenId = "o1";
                mi.MemberType = IQBWX.Common.MemberType.City;
                mi.RegisterDateTime = DateTime.Now;
                mi.UserId = 2;
                db.MemberInfo.Add(mi);

                mi = new EMemberInfo();
                mi.openId = "o3";
                mi.ParentOpenId = "o2";
                mi.UserId = 3;
                mi.MemberType = IQBWX.Common.MemberType.Channel;
                mi.RegisterDateTime = DateTime.Now;
                db.MemberInfo.Add(mi);

                mi = new EMemberInfo();
                mi.openId = "o4";
                mi.UserId = 4;
                mi.ParentOpenId = "o3";
                mi.MemberType = IQBWX.Common.MemberType.City;
                mi.RegisterDateTime = DateTime.Now;
                db.MemberInfo.Add(mi);

                mi = new EMemberInfo();
                mi.openId = "o5";
                mi.UserId = 5;
                mi.ParentOpenId = "o4";
                mi.MemberType = IQBWX.Common.MemberType.Channel;
                mi.RegisterDateTime = DateTime.Now;
                db.MemberInfo.Add(mi);

                db.SaveChanges();

            }
        }
    }
}
