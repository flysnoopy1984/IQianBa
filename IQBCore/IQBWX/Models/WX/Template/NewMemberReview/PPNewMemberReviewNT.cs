using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.WX.Template.NewMemberReview
{
    public class PPNewMemberReviewNT : Notification<PPNewMemberReviewTemplate>
    {
        string _OpenId;
        string _newOpenId;
        string _newName;
        string _JoinDateTime;

        public PPNewMemberReviewNT(string accessToken,
                                   string toUserOpenId,
                                   string newOpenId,
                                   string newName,
                                   string JoinDateTime) : base(accessToken)
        {
            _OpenId = toUserOpenId;
            _newOpenId = newOpenId;
            _newName = newName;
            _JoinDateTime = JoinDateTime;
        }

        protected override PPNewMemberReviewTemplate SetData()
        {
            PPNewMemberReviewTemplate data = new PPNewMemberReviewTemplate();
            data = data.GenerateData(_OpenId,_newOpenId,_newName,_JoinDateTime);
            return data;
        }
    }
}
