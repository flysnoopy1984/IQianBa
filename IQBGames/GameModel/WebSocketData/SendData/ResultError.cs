using GameModel.Message.SendData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;
using GameModel.Message;

namespace GameModel.WebSocketData.SendData
{
    public class ResultError : BaseSendMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.GameError;
            }
        }

      

        public ResultError(string errorMsg,string roomCode = null)
        {
            ErrorMsg = errorMsg;

            if (roomCode !=null)
            {
                IGameMessage ig = this;
                ig.MessageSendTarget.SendTarget = SendTarget.UserInRoom;
                ig.MessageSendTarget.TargetRoom = roomCode;
            }
           
        }
    }
}
