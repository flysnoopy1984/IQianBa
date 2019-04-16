using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public class EGameCoins
    {


        public EGameCoins(string rc)
        {
            RoomCode = rc;
        }
        public string RoomCode { get; set; }

        public Dictionary<int,decimal> PileCoins { get; set; }

        protected Dictionary<int, List<ECoinDetail>> PlayerCoinsDetail { get; set; }

        public void AddCoins(ECoinDetail pc)
        {
            int pileNum = pc.PileNo;

            if (PlayerCoinsDetail == null)
                PlayerCoinsDetail = new Dictionary<int, List<ECoinDetail>>();
            PlayerCoinsDetail[pileNum].Add(pc);

            if (PileCoins == null)
                PileCoins = new Dictionary<int, decimal>();
            PileCoins[pileNum] = PileCoins[pileNum] + pc.Coins;

        }

    }
}
