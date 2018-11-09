using IQBCore.IOS.APNS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace IQBConsole
{
    class Program
    {
        static PPBatchJob job = new PPBatchJob();
        static PayTest payTest = new PayTest();

        static int _OrderDiffMin = 90;
        static int _interval = 60;

        static void Main(string[] args)
        {
          
            try
            {
                //string jsonStr = "";// " 菜单结构";
                // string jsonPath = @"c:\message.json";
                // using (StreamReader sr = new StreamReader(jsonPath))
                // {
                //     jsonStr = sr.ReadToEnd();
                // }

                // string cerPath = @"C:\OOProd.p12";
                // IOSPushMessage message = new IOSPushMessage(IOSPushType.Development, cerPath, "edifier");
                // IOSPushSetting pushSetting = new IOSPushSetting();
                // pushSetting.deviceToken = "bb2288cbc4f29bf1dcb32ed6709f342404b882e7c49200de061dc992a4ef2ae4";
                // pushSetting.message = jsonStr ;// " 菜单结构";

                // pushSetting.sound = "default";
                // pushSetting.badge = 1;

                // message.Push(pushSetting);

                PushSharp ps = new PushSharp();
                ps.StartServer();

                ps.SendMsg();

               

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
            finally
            {
              
            }
            
        }



       

      
    }
}
