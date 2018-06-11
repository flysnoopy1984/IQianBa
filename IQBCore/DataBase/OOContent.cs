using IQBCore.OO.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.DataBase
{
    public class OOContent: DbContext
    {
        public OOContent() : base("OOConnection")
        {

        }
        public DbSet<EBanner> DBBanner { get; set; }

    }
}
