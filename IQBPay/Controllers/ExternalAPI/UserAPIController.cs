using IQBPay.DataBase;
using IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBPay.Controllers.ExternalAPI
{
    public class UserAPIController : ApiController
    {
        [HttpPost]
        public string Register([FromBody]EUserInfo ui)
        {
            try
            {
                if (ui != null)
                {
                    using (AliPayContent db = new AliPayContent())
                    {
                        if (db.IsExistUser(ui.OpenId))
                        {
                            return "EXIST";
                        }
                        else
                        {
                            db.UserInfoDB.Add(ui);
                            db.SaveChanges();
                            return "OK";
                        }
                    }
                }
                else
                    return "参数传入失败！";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
