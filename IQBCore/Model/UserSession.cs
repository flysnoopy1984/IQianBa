using IQBCore.IQBPay.BaseEnum;
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

        public string Headimgurl { get; set; }

        public string Name { get; set; }


    }
}
