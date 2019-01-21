using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.WSJsonData
{
    public class jdNewConnect:BaseWSJsonData
    {
        public string OpenId { get; set; }

        public string RoomCode { get; set; }
    }
}
