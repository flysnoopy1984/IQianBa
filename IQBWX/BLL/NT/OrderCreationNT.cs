using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.Models.Order;
using IQBWX.Models.Product;
using IQBWX.Models.User;
using IQBCore.IQBWX.Models.WX.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQBWX.Models.WX.Template;

namespace IQBWX.BLL.NT
{
    /// <summary>
    /// 下单未支付发送通知到上级
    /// </summary>
    public class OrderCreationNT : Notification<OrderCreationTemplate>
    {
        private EUserInfo _UserInfo;
        private EMemberInfo _pMemberInfo;
        private EOrderLine _Order;

        public OrderCreationNT(EUserInfo ui, EOrderLine order, string accessToken)
            :base(accessToken)
        {
            _UserInfo = ui;
            _Order = order;
            using (UserContent db = new UserContent())
            {
                _pMemberInfo = db.GetMemberInfoByOpenId(_UserInfo.ParentOpenId);
            }
        }

        protected override OrderCreationTemplate SetData()
        {
            MemberType mt = _Order.ItemId == EItemInfo.Item158 ? MemberType.Channel : MemberType.City;

            OrderCreationTemplate data = new OrderCreationTemplate();
            decimal commission = PopPolicy.GetPopLevelCommission(mt, _pMemberInfo.MemberType);
            data = data.GenerateData(_UserInfo.ParentOpenId, _Order, _UserInfo.nickname, 0);
            return data;
        }
    }
}