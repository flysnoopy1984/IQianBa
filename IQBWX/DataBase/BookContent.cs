using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBWX.DataBase
{
    public class BookContent: DbContext
    {
        public BookContent() : base("BookDBConnection")
        {

        }

        
    }
}