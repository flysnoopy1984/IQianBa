using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Model
{
    public abstract class BaseModel
    {

        [MaxLength(20)]
        public string CreateUser { get; set; }
        [MaxLength(20)]
        public string CDate { get; set; }
        [MaxLength(20)]
        public string CTime { get; set; }
       
        public DateTime CreateDate { get; set; }
        [MaxLength(20)]
        public string ModifyUser { get; set; }
        [MaxLength(20)]
        public string MDate { get; set; }
        [MaxLength(20)]
        public string MTime { get; set; }
        
        public DateTime ModifyDate { get; set; }

        [NotMapped]
        public string RunResult { get; set; }

        public BaseModel()
        {
            this.CDate = DateTime.Now.ToShortDateString();
            this.CTime = DateTime.Now.ToShortTimeString();
            CreateDate = DateTime.Now;

            this.MDate = DateTime.Now.ToShortDateString();
            this.MTime = DateTime.Now.ToShortTimeString();
            ModifyDate = DateTime.Now;
        }

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
