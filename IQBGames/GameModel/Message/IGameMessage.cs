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
        MessageSendTarget SendTarget { get; set; }
        string GetMessage();
    }
}
