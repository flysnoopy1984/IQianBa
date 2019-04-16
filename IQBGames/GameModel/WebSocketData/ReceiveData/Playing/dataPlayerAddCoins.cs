using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.WebSocketData.ReceiveData.Playing
{
    public class dataPlayerAddCoins: BaseReceiveData
    {
        public decimal AddCoins { get; set; }
    }
}
