using GameModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Message
{
    public interface IGameMessage
    {
        MessageType MessageType { get; }
        MessageSendTarget MessageSendTarget { get;}

        string SessionId { get; set; }
        string GetMessage();
    }
}
