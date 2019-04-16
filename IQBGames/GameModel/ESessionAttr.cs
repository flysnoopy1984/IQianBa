using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public class ESessionAttr
    {
        
        public ESessionAttr()
        {
           
        }
        //   public string OpenId { get; set; }

        //     public string UserName { get; set; }

        public int Weight { get; set; }

        public string UserName { get; set; }

        public string UserOpenId { get; set; }

        public string RoomCode { get; set; }

        //private ERoomUser _RoomUser = null;
        //public ERoomUser RoomUser
        //{
        //    get
        //    {
        //        if (_RoomUser == null)
        //            _RoomUser = new ERoomUser();
        //        return _RoomUser;
        //    }
        //}
    }
}
