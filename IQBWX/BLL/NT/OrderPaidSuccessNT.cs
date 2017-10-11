using IQBWX.Common;
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
    public class OrderPaidSuccessNT : Notification<OrderPaidSuccessTemplate>
    {
        private EOrderLine _order;
        private EMemberInfo _currentMemberInfo;
        private EMemberInfo _parentMemberInfo;

        public OrderPaidSuccessNT(string accessToken,EOrderLine order,EMemberInfo mi,EMemberInfo pmi)
            :base(accessToken)
        {
            _order = order;
            _currentMemberInfo = mi;
            _parentMemberInfo = pmi;


        }

        protected override OrderPaidSuccessTemplate SetData()
        {
            OrderPaidSuccessTemplate data = new OrderPaidSuccessTemplate();
            decimal commission = PopPolicy.GetPopLevelCommission(_currentMemberInfo.MemberType,
                                                                                                            _parentMemberInfo.MemberType);

            data = data.GenerateData(_currentMemberInfo,_parentMemberInfo,_order, commission);
            return data;
        }
    }
}