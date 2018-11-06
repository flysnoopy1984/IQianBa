using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace IQBConsole
{
    public class PushToAPNs
    {
        /// <summary>
        /// 苹果信息推送 证书 路径（注意测试版跟正式发布版证书上不一样）
        /// </summary>
        private static string cerPath = ConfigurationManager.AppSettings["CerPath"];
        /// <summary>
        /// 苹果推送服务 密码
        /// </summary>
        private static string msgPushPWD = ConfigurationManager.AppSettings["MSGPushPWD"];
        /// <summary>
        /// 苹果推送服务 开关
        /// open:开 close:关
        /// </summary>
        private static string msgPushSwitch = ConfigurationManager.AppSettings["MSGPushSwitch"];
        /// <summary>
        /// 苹果消息推送 请求地址 产品正式版：gateway.push.apple.com  测试环境：gateway.sandbox.push.apple.com
        /// </summary>
        private static string iosHostUrl = ConfigurationManager.AppSettings["iosHostUrl"];
        /// <summary>
        /// 苹果消息推送 请求端口
        /// </summary>
        private static string iosHostPort = ConfigurationManager.AppSettings["iosHostPort"];

        public static DateTime? Expiration { get; set; }
        public static readonly DateTime DoNotStore = DateTime.MinValue;
        private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static string DeviceToken = "";//;//苹果设备token
        public const int DEVICE_TOKEN_BINARY_SIZE = 32;
        public const int DEVICE_TOKEN_STRING_SIZE = 64;
        public const int MAX_PAYLOAD_SIZE = 256;
        private static X509Certificate certificate;
        private static X509CertificateCollection certificates;
        public static string apnsMessage = "测试内容！！";
        /// <summary>
        /// 发送消息
        /// </summary>
        public static void SendMessage()
        {
            //苹果推送开关
            if (msgPushSwitch == "close")
            {
                return;
            }
            string hostIP = iosHostUrl;//"gateway.push.apple.com";//"gateway.sandbox.push.apple.com";//
            int port = int.Parse(iosHostPort);
            string password = string.Empty;
            string certificatepath = string.Empty;
            password = msgPushPWD;//"123456";
            certificatepath = cerPath;//"Resources\\pushCerUse.p12";
            string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, certificatepath);
            certificate = new X509Certificate2(System.IO.File.ReadAllBytes(p12Filename), password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            certificates = new X509CertificateCollection();
            certificates.Add(certificate);
            TcpClient apnsClient = new TcpClient();
            apnsClient.Connect(hostIP, port);
            SslStream apnsStream = new SslStream(apnsClient.GetStream(), false, new RemoteCertificateValidationCallback(validateServerCertificate), new LocalCertificateSelectionCallback(selectLocalCertificate));
            try
            {
                //APNs已不支持SSL 3.0
                apnsStream.AuthenticateAsClient(hostIP, certificates, System.Security.Authentication.SslProtocols.Tls, false);
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                Console.WriteLine("error+" + ex.Message);
            }
            if (!apnsStream.IsMutuallyAuthenticated)
            {
                Console.WriteLine("error:Ssl Stream Failed to Authenticate！");
            }
            if (!apnsStream.CanWrite)
            {
                Console.WriteLine("error:Ssl Stream is not Writable!");
            }
            Byte[] message = ToBytes();
            apnsStream.Write(message);
        }
        public static byte[] ToBytes()
        {
            // Without reading the response which would make any identifier useful, it seems silly to
            // expose the value in the object model, although that would be easy enough to do. For
            // now we'll just use zero.
            int identifier = 0;
            byte[] identifierBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(identifier));
            // APNS will not store-and-forward a notification with no expiry, so set it one year in the future
            // if the client does not provide it.
            int expiryTimeStamp = -1;//过期时间戳
            if (Expiration != DoNotStore)
            {
                //DateTime concreteExpireDateUtc = (Expiration ?? DateTime.UtcNow.AddMonths(1)).ToUniversalTime();
                DateTime concreteExpireDateUtc = (Expiration ?? DateTime.UtcNow.AddSeconds(20)).ToUniversalTime();
                TimeSpan epochTimeSpan = concreteExpireDateUtc - UNIX_EPOCH;
                expiryTimeStamp = (int)epochTimeSpan.TotalSeconds;
            }
            byte[] expiry = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(expiryTimeStamp));
            byte[] deviceToken = new byte[DeviceToken.Length / 2];
            for (int i = 0; i < deviceToken.Length; i++)
                deviceToken[i] = byte.Parse(DeviceToken.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            if (deviceToken.Length != DEVICE_TOKEN_BINARY_SIZE)
            {
                Console.WriteLine("Device token length error！");
            }
            byte[] deviceTokenSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(deviceToken.Length)));
            string str = "{\"aps\":{\"alert\":\"" + apnsMessage + "\",\"badge\":1,\"sound\":\"anke.mp3\"}}";
            byte[] payload = Encoding.UTF8.GetBytes(str);
            byte[] payloadSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt16(payload.Length)));
            List<byte[]> notificationParts = new List<byte[]>();
            //1 Command
            notificationParts.Add(new byte[] { 0x01 }); // Enhanced notification format command
            notificationParts.Add(identifierBytes);
            notificationParts.Add(expiry);
            notificationParts.Add(deviceTokenSize);
            notificationParts.Add(deviceToken);
            notificationParts.Add(payloadSize);
            notificationParts.Add(payload);
            return BuildBufferFrom(notificationParts);
        }
        private static byte[] BuildBufferFrom(IList<byte[]> bufferParts)
        {
            int bufferSize = 0;
            for (int i = 0; i < bufferParts.Count; i++)
                bufferSize += bufferParts[i].Length;
            byte[] buffer = new byte[bufferSize];
            int position = 0;
            for (int i = 0; i < bufferParts.Count; i++)
            {
                byte[] part = bufferParts[i];
                Buffer.BlockCopy(bufferParts[i], 0, buffer, position, part.Length);
                position += part.Length;
            }
            return buffer;
        }
        private static bool validateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; // Dont care about server's cert
        }
        private static X509Certificate selectLocalCertificate(object sender, string targetHost, X509CertificateCollection localCertificates,
         X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return certificate;
        }
    }
}
