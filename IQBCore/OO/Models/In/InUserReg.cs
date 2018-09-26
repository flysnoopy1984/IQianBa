using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.In
{
    public class InUserReg
    {
        public string Phone { get; set; }

        public string LoginName { get; set; }

        public string Pwd { get; set; }

        public string NickName { get; set; }

        /// <summary>
        /// 用户上级邀请码
        /// </summary>
        public string InviteCode { get; set; }

    }
}
