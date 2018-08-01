using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.OutParameter;
using IQBWX.DataBase.IQBPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBWX.Controllers.API
{
    public class PPUserController : ApiController
    {

        private void ChangeUserStatus(string openId,UserStatus userStatus)
        {
           
            using (AliPayContent db = new AliPayContent())
            {
                string sql = string.Format("update userInfo set UserStatus={0}", (int)userStatus);
                db.Database.ExecuteSqlCommand(sql);
            }
        }
        [HttpGet]
        public OutAPIResult AgreeNewMember(string OpenId)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                ChangeUserStatus(OpenId, UserStatus.PPUser);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpGet]
        public OutAPIResult DisAgreeNewMember(string OpenId)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                ChangeUserStatus(OpenId, UserStatus.WaitRewiew);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
    }
}
