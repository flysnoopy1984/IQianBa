using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Model
{
    public class UserSession
    {

        public int Id { get; set; }

        public string OpenId { get; set; }

        public UserRole UserRole { get; set; }

        public UserStatus UserStatus { get; set; }

        public string Headimgurl { get; set; }

        public string Name { get; set; }

        public bool HasQRHuge { get; set; }

        public bool HasPassInviteFee { get; set; }

        public bool HasO2OEntry { get; set; }

        public O2OUserRole O2OUserRole { get; set; }

        public string AgentPhone { get; set; }

        public string AliPayAccount { get; set; }

        public void InitFromUser(EUserInfo ui)
        {
            HasQRHuge = ui.HasQRHuge;
            Id = ui.Id;
            OpenId = ui.OpenId;
            UserRole = ui.UserRole;
            Headimgurl = ui.Headimgurl;
            Name = ui.Name;
            HasPassInviteFee = ui.HasPassInviteFee;
            HasQRHuge = ui.HasQRHuge;
            O2OUserRole = ui.O2OUserRole;
            AgentPhone = ui.UserPhone;
            UserStatus = ui.UserStatus;

            AliPayAccount = ui.AliPayAccount;

        }


    }
}
