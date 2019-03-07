using GameModel.WebSocketData.ReceiveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Message;

namespace GameServer.Command
{
    public class BackHall : BaseGameCommand<dataBackHall>
    {
        public override List<BaseNormalMsg> HandleData(GameUserSession session, dataBackHall Data)
        {
            throw new NotImplementedException();
        }

        public override bool VerifyCommandData(dataBackHall InData)
        {
            throw new NotImplementedException();
        }
    }
}
