using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBWX.Controllers.API
{
    public class SMSController : ApiController
    {
        [HttpGet]
        public string getVerifyDiffSec(int userId)
        {
            int result;
            try
            {
                using (UserContent db = new UserContent())
                {
                    result =  db.GetVerifyDiff(userId, Convert.ToString(SMSEvent.NewMember));
                    
                }
            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }
            return result.ToString();
        }

        [HttpGet]
        public string NewMemberSMSVerify(int userId)
        {
            try { 
                using (UserContent db = new UserContent())
                {
                    EUserSMSVerify uv = new EUserSMSVerify()
                    {
                        SendDateTime = DateTime.Now,
                        VerifyCode = StringHelper.GetRnd(6, true, true, false, false, ""),
                        UserId = userId,
                        VerifyStatus = UserSMSStatus.Sent,
                        SMSEvent = Convert.ToString(SMSEvent.NewMember)
                    };
                    this.DoSMS();
                    db.InsertEUserSMSVerify(uv);
                }
            }
            catch(Exception ex)
            {
                return "error:" + ex.Message;
            }
            return "OK";
        }

        private void DoSMS()
        {

        }
    }
}
