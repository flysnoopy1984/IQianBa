using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.DataBase
{
    public class OOTContent<T>:DbContext where T:class
    {
        public OOTContent(): base("OOConnection")
        {

        }

        public DbSet<T> Db { get; set; }
    }
}
