using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.Json.WXMedia
{
    public class JOMedia
    {
        public string total_count { get; set; }
        public string item_count { get; set; }

        public List<JOMedia_Item> item { get; set; }
    }
}
