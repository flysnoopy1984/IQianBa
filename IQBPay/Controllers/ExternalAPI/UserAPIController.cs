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
        [HttpGet]
        public string Register([FromBody]EUserInfo ui)
        {
            string openId = ui.OpenId;


            return "OK";
            

            
        }
    }
}
