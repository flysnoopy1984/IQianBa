using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBCore.Common.Helper
{
    public class StringHelper
    {
        public static string GetRnd(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;

            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }

            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }

            return s;
        }

        public static string urlconvertor(string WebRoot, string imagesurl1)
        {

            string imagesurl2 = imagesurl1.Replace(WebRoot, ""); //转换成相对路径
            imagesurl2 = imagesurl2.Replace(@"\", @"/");
            return "/"+imagesurl2;
        }

        public static string GenerateOrderNo()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmss") + GetRnd(8,true,true,false,false,"");
        }

        public static string GenerateSubAccountTransNo()
        {
            return DateTime.Now.ToString("SubyyyyMMddhhmmss") + GetRnd(8, true, true, false, false, "");
        }
    }
}