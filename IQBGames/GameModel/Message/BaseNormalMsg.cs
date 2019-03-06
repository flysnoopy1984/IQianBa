using GameModel.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Message
{
    public abstract class BaseNormalMsg: IGameMessage
    {
        public abstract GameActionCode Action { get;}

        private MessageType _MessageType;
        public BaseNormalMsg()
        {
            _MessageType = MessageType.Normal;
            _SendTarget = MessageSendTarget.Self;
        }
    
        private string _ErrorMsg;
        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set
            {

                // IsSuccess = false;
                 _MessageType = MessageType.Error;
                 _ErrorMsg = value;
            }
        }

        public MessageType MessageType
        {
            get
            {
                return _MessageType;
            }
        }

        private MessageSendTarget _SendTarget; 
        public MessageSendTarget SendTarget
        {
            get{return _SendTarget;}
            set{_SendTarget = value;}
        }

        string IGameMessage.GetMessage()
        {
            if (MessageType == MessageType.Normal)
                return JsonConvert.SerializeObject(this);
            else
            {
                var errorMsg = new MessageNormalError()
                {
                    ErrorMsg = this.ErrorMsg
                };
                return JsonConvert.SerializeObject(errorMsg);
            }
        }
    }
}
