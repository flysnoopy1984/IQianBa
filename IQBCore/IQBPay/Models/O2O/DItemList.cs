using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    public class DItemList
    {
        private List<HashO2OMall> _HashO2OMall;
        public List<HashO2OMall> HashO2OMall
        {
            get
            {
                if (_HashO2OMall == null)
                    _HashO2OMall = new List<HashO2OMall>();
                return _HashO2OMall;
            }
            set { _HashO2OMall = value; }
        }

        private List<HashO2ORule> _HashO2ORule;
        public List<HashO2ORule> HashO2ORule
        {
            get
            {
                if (_HashO2ORule == null)
                    _HashO2ORule = new List<HashO2ORule>();
                return _HashO2ORule;
            }
            set { _HashO2ORule = value; }
        }

        public List<RO2ODeliveryAddr> AddrList { get; set; }
    }
}
