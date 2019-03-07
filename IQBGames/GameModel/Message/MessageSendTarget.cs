using GameModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Message
{
    public class MessageSendTarget
    {
        public SendTarget SendTarget { get; set; }
        public MessageSendTarget()
        {
            SendTarget = SendTarget.Self;
        }

        public string TargetOpenId {get;set; }

        public string TargetRoom { get; set; }
       
    }
}
