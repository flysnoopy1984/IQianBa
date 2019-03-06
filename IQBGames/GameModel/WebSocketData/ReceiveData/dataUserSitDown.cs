using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.WebSocketData.ReceiveData
{
    public class dataUserSitDown:BaseWSJsonData
    {
        public string OpenId { get; set; }

        public int SeatNo { get; set; }

        public decimal Coins { get; set; }
    }
}
