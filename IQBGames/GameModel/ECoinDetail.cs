using GameModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public class ECoinDetail
    {
        public decimal Coins { get; set; }

        public decimal Diff { get; set; }

        public string UserOpenId { get; set; }

        public int PileNo { get; set; }

        public CoinType CoinType { get; set; }
    }
}
