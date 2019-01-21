using IQBCore.OO.BaseEnum;
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

        //public string DeviceToken { get; set; }

        public string DeviceIdentify { get; set; }

        //public DeviceChannel DeviceChannel { get; set; }

        public string AppName { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        /// 用户上级邀请码
        /// </summary>
        public string InviteCode { get; set; }

    }
}
