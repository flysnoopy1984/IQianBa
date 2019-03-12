
using GameModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public class ECard
    {
        /// <summary>
        /// 内部编号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 大小JQKA 11, 12, 13,14
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 黑桃 红心 方块 红桃
        /// </summary>
        public CardType CardType { get; set; }
    }
}
