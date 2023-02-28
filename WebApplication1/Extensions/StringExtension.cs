using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Extensions
{
    public static class StringExtension
    {
        public static string HashSha1(this string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hashSh1 = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hashSh1.Length * 2);
                foreach (byte b in hashSh1)
                {
                    sb.Append(b.ToString("X2").ToLower());
                }
                return sb.ToString();
            }

        }
    }
}
