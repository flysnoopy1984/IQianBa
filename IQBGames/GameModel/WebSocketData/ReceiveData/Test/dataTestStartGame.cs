using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.WebSocketData.ReceiveData.Test
{
    public class dataTestStartGame: BaseReceiveData
    {
        public string RoomCode { get; set; }
    }
}
