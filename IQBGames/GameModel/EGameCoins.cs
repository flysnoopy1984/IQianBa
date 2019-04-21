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

        /// <summary>
        /// 筹码概括
        /// </summary>
        public Dictionary<int,decimal> PileCoins { get; set; }

        /// <summary>
        /// 筹码明细
        /// </summary>
        protected Dictionary<int, List<ECoinDetail>> PlayerCoinsDetail { get; set; }

        public void AddCoins(ECoinDetail pc)
        {
            int pileNum = pc.PileNo;

            if (PlayerCoinsDetail == null)
                PlayerCoinsDetail = new Dictionary<int, List<ECoinDetail>>();
            if (!PlayerCoinsDetail.ContainsKey(pileNum))
                PlayerCoinsDetail.Add(pileNum, new List<ECoinDetail>());
           
            PlayerCoinsDetail[pileNum].Add(pc);

            if (PileCoins == null)
                PileCoins = new Dictionary<int, decimal>();
            if (!PileCoins.ContainsKey(pileNum))
                PileCoins.Add(pileNum, pc.Coins);
            else
                PileCoins[pileNum] = PileCoins[pileNum] + pc.Coins;

        }

    }
}
