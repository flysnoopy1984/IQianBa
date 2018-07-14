using IQBCore.IQBRecharge.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBRecharge.Models.In
{
    public class InOrder
    {
        public CardType CardType { get; set; }

        public double CardValue {get;set;}

        public string CardNo { get; set; }

        public string CardPwd { get; set; }
    }
}
