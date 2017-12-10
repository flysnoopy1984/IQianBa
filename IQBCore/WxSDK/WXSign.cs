using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.WxSDK
{
    public class WXSign
    {
        public string timestamp { get; set; }
        public string AppId { get; set; }

        public string nonceStr { get; set; }

        public string signature { get; set; }

    }
}
