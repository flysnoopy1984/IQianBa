using IQBCore.IQBPay.Models.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.BLL
{
    public class RuleManager
    {
        private static EPayRule _PayRule;

        private static Dictionary<int, EBenefitRule> _BenefitRule;

        public static EPayRule PayRule()
        {
            if(_PayRule == null)
            {
                _PayRule = new EPayRule();
                _PayRule.Agent_InviteFee = 999;
                _PayRule.Agent_QRHugeFee = 5000;
                _PayRule.Agent_FOFeeRate = 2.8;
                _PayRule.User_ServerFee_Q = 2;
                _PayRule.User_ServerFee_HQ = 2;
            }
            return _PayRule;
        } 

        public static Dictionary<int,EBenefitRule> BenefitRule()
        {
            if(_BenefitRule == null)
            {
                _BenefitRule = new Dictionary<int, EBenefitRule>();
                _BenefitRule.Add(1, new EBenefitRule());
                _BenefitRule[1].FeeRate = 1.5;
                _BenefitRule[1].CommRate = 0;
                _BenefitRule[1].L3CommAmtRate = 0;
                _BenefitRule[1].Level = 1;

                _BenefitRule.Add(2, new EBenefitRule());
                _BenefitRule[2].FeeRate = 1.8;
                _BenefitRule[2].CommRate = 0.2;
                _BenefitRule[2].L3CommAmtRate = 0.3;
                _BenefitRule[2].Level = 2;

                _BenefitRule.Add(3, new EBenefitRule());
                _BenefitRule[3].FeeRate = 2;
                _BenefitRule[3].CommRate = 0.2;
                _BenefitRule[3].L3CommAmtRate = 0.3;
                _BenefitRule[3].Level = 3;


            }
            return _BenefitRule;
        }

        public static void ResetRule()
        {
            _BenefitRule = null;
            _PayRule = null;
        }
    }
}

