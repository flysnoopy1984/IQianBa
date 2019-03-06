using GameModel.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;

namespace GameModel.WebSocketData.SendData
{
    public class ResultUserSitDown:BaseNormalMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.PlayerSitDown;
            }
        }

        public int SeatNo { get; set; }


    }
}
