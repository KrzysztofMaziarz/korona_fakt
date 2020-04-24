using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FakeNewsCovid.Domain.Extensions
{
    public class GuidExtension
    {
        public static System.Guid NewDeterministicGuid(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] textBytes = Encoding.Default.GetBytes(text);
            byte[] hash = md5.ComputeHash(textBytes);
            return new System.Guid(hash);
        }
    }
}
