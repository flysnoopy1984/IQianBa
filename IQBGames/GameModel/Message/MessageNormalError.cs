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
        MessageSendTarget IGameMessage.MessageSendTarget
        {
            get
            {
                if (_SendTarget == null)
                    _SendTarget = new MessageSendTarget();
                return _SendTarget;
            }
        }

        public MessageNormalError() { }
        public MessageNormalError(string error)
        {
            ErrorMsg = error;
        }
        string IGameMessage.GetMessage()
        {
            return JsonConvert.SerializeObject(this);
        }
        private string _SessionId = null;
        public string SessionId
        {
           get { return _SessionId; }
           set { _SessionId = value; }
        }
    }
}
