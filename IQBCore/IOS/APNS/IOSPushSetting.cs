using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IOS.APNS
{
    public class IOSPushSetting
    {
        public string deviceToken;
        public string message;
        public string sound;
        public int badge;

        public string PushPayload()
        {
            return "{\"aps\":{\"alert\":\"" + message + "\",\"badge\":" + badge + ",\"sound\":\"" + sound + "\"}}";
        }
    }

    public enum IOSPushType
    {
        Development,
        Distribution
    }
}
