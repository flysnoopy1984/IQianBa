using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.WebSocketData.ReceiveData
{
    public class dataNewConnect:BaseWSJsonData
    {
        public string OpenId { get; set; }

        public string RoomCode { get; set; }

        public string UserName { get; set; }

        public int Weight { get; set; }
    }
}
