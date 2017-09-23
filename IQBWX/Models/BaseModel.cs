using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models
{
    public class BaseModel
    {

        public string CreateUser { get; set; }
        public string CDate { get; set; }
        public string CTime { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyUser { get; set; }
        public string MDate { get; set; }
        public string MTime { get; set; }
        public DateTime ModifyDate { get; set; }

        public void InitCreate()
        {
            this.CDate = DateTime.Now.ToShortDateString();
            this.CTime = DateTime.Now.ToShortTimeString();
            CreateDate = DateTime.Now;
        }

        public void InitModify()
        {
            this.MDate = DateTime.Now.ToShortDateString();
            this.MTime = DateTime.Now.ToShortTimeString();
            ModifyDate = DateTime.Now;
        }

    }
}