using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Model;
using IQBCore.OO.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBAPI.Controllers
{
    public class UserController : BaseAPIController
    {
        /// <summary>
        /// 用户登录，返回用户基本信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public OutAPIResult Login(string userName,string pwd)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {

            }
            catch(Exception ex)
            {

            }

            return result;
        }

        /// <summary>
        /// 用户团队成员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public NResult<EUserInfo> QueryTeamMember()
        {
            NResult<EUserInfo> result = new NResult<EUserInfo>();
            try
            {

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        /// <summary>
        /// 获取用户邀请信息
        /// </summary>
        /// <returns></returns>
        public OutAPIResult UserInviteInfo()
        {
            OutAPIResult result = new OutAPIResult();
            try
            {

            }
            catch(Exception ex)
            {

            }
            return result;
        }

    }
}
