using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Results
{
    public class RSSOResult
    {
        public string ssoToken { get; set; }

        public string AppId { get; set; }

        public  string OpenId { get; set; }

        public string ReturnUrl { get; set; }

        public string ErrorMsg { get; set; }

    }
}