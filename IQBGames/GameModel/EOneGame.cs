using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public class EOneGame
    {
        public string RoomCode { get; set; }
        public List<int> TableCardList { get; set; }

        

        //当前按钮位置
        public int CurD { get; set; }

     
    }
}
