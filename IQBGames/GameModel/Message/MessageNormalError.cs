using GameModel.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Message
{
    public class MessageNormalError: IGameMessage
    {
       
        public string ErrorMsg { get; set; }

        public MessageType MessageType
        {
            get
            {
                return MessageType.Error;
            }
        }

        private MessageSendTarget _SendTarget;
        public MessageSendTarget SendTarget
        {
            get { return _SendTarget; }
            set { _SendTarget = value; }
        }

        string IGameMessage.GetMessage()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
