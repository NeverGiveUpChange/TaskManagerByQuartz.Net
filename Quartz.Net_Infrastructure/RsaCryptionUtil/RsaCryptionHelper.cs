using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Quartz.Net_Infrastructure.RsaCryptionUtil
{
    public static class  RsaCryptionHelper
    {
        public static string Base64Encode(this string encryptString)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(encryptString);
            return Convert.ToBase64String(encbuff);
        }

        public static string Base64Decode(this string decryptString)
        {
            byte[] decbuff = Convert.FromBase64String(decryptString);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        public static string RSAEncrypt(this string s, string key)
        {
            if (string.IsNullOrEmpty(s)) throw new ArgumentException("An empty string value cannot be encrypted.");

            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(key);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(s), false);
            return HttpUtility.UrlEncode(Convert.ToBase64String(cipherbytes));
        }

        public static string MD5(this string str)
        {
            string cl1 = str;
            string pwd = "";
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();// 加密后是一个字节类型的数组 
            byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(cl1));
            for (int i = 0; i < s.Length; i++)
            {// 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得 
                pwd = pwd + s[i].ToString("x");// 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
            }
            return pwd;
        }
        public static string MD5CSP(this string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);

            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToLower();
            return retStr;
        }
        public static string SHA256(this string str)
        {
            byte[] sourceByte = Encoding.UTF8.GetBytes(str);
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] cryByte = sha256.ComputeHash(sourceByte);
            return _byteToHexStr(cryByte);
        }

        private static  string _byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr.ToLower();
        }

        /// <summary>
        /// 调用api请求方法时用
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetTimeStmap()
        {
            return Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds, 2).ToString();
        }

    }
}
