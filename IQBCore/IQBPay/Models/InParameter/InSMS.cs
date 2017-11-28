using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.InParameter
{
    public class InSMS
    {
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Parameters{get;set;}

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 模板编号
        /// </summary>
        public string Tpl_id { get; set; }

        public void Init()
        {
           
            this.Sign = "玉杰投资服务";
            //   this.Tpl_id = "49551"; //您的验证码为:{1}.您的订单编号为:{2}.请在支付后到以下地址进行收款确认:{3}。
          //  this.Tpl_id = "50959";//您的验证码为:{1}.您的收款确认码为:{2}.，请在支付后到以下地址用收款确认码进行收款确认:{3}。

        }
    }
}
