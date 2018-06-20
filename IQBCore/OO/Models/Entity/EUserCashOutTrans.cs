using IQBCore.OO.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("UserCashOutTrans")]
    public class EUserCashOutTrans:EBaseRecord
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public double TransAmount { get; set; }

        public CashTransType CashTransType { get; set; }
    }
}
