using GameModel.Message.SendData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;

namespace GameModel.WebSocketData.SendData
{
    public class ResultGameShuffling : BaseSendMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.Shuffling;
            }
        }

        public List<ECard> PlayerCards { get; set; }
    }
}
