using GameModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public class ESeatUser
    {
        public int SeatNo { get; set; }

        public string UserOpenId { get; set; }

        public SeatStauts SeatStauts { get; set; }
    }
}
