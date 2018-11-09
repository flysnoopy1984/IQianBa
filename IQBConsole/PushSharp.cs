using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBConsole
{
    public class PushSharp
    {
        private ApnsConfiguration config;
        private ApnsServiceBroker apnsBroker;

        public void StartServer()
        {
            config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox, "OOPush.p12", "edifier");
            apnsBroker = new ApnsServiceBroker(config);

            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    //判断例外，进行诊断
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException)ex;
                        //处理失败的通知 
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;
                        Console.WriteLine("Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}" + notification.DeviceToken);
                    }
                    else
                    {
                        //内部异常
                        Console.WriteLine("Apple Notification Failed for some unknown reason : {ex.InnerException}" + notification.DeviceToken);
                    }
                    // 标记为处理
                    return true;
                });
            };
            //推送成功
            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("Apple Notification Sent ! " + notification.DeviceToken);
            };

            apnsBroker.Start();

        }

        public void SendMsg()
        {
            List<string> MY_DEVICE_TOKENS = new List<string>() {
                "bb2288cbc4f29bf1dcb32ed6709f342404b882e7c49200de061dc992a4ef2ae4"
            };

            foreach (var deviceToken in MY_DEVICE_TOKENS)
            {
                // 队列发送一个通知
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken,//这里的deviceToken是ios端获取后传递到数据库统一记录管理的，有效的Token才能保证推送成功
                    Payload = JObject.Parse("{\"aps\":{\"sound\":\"default\",\"badge\":\"1\",\"alert\":\"这是一条群发广告消息推送的测试消息\"}}")
                });
            }
            Console.WriteLine("Sent");

            //停止代理
            apnsBroker.Stop();
            Console.Read();
        }


    }
}
