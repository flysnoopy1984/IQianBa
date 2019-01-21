using IQBCore.IQBPay.Models.SMS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.DataBase
{
    public class PPContent:DbContext
    {
        public PPContent():base("PPConnection")
        {

        }

        public DbSet<ESMSVerification> DBSMSVerification { get; set; }

        public DbSet<ESMSLog> DBSMSLog { get; set; }
    }
}
