using System;
using System.Security.Cryptography;
using System.Text;

namespace DKS_API.Helpers
{
    public class CSharpLab
    {
        //加解密範例程式
        //加密演算法 AES
        //預設參數:
        //Mode:CBC , key size: 32 Bytes(256bits) , block size: 16 Bytes(128bits) , padding: PKCS7 
        public static void Test()
        {

            var testStr = "CB\\dieu.hien";
            var testkey = "TalentPool";
            var testiv = "SSB";
            //testStr = Uri.UnescapeDataString(testStr);

            testStr = encrypt(testStr, testkey, testiv);
            testStr = Uri.EscapeDataString(testStr);

            //string encodedUrl = Uri.EscapeDataString(decodedUrl);

            //var d = decrypt(testStr, testkey, testiv);
            //Console.WriteLine(d);
        }

        //加密
        private static string encrypt(string data, string strKey, string strIV)
        {

            //將key轉成utf8編碼 byte array
            byte[] tmpkey = System.Text.Encoding.UTF8.GetBytes(strKey);

            //將iv轉成utf8編碼 byte array
            byte[] tmpIV = System.Text.Encoding.UTF8.GetBytes(strIV);

            MD5CryptoServiceProvider mD5Provider = new MD5CryptoServiceProvider();

            //將tempkey取得md5的hash,結果會是16 byte的array            
            byte[] key = mD5Provider.ComputeHash(tmpkey);

            //將tmpIV取得md5的hash,結果會是16 byte的array  
            byte[] iv = mD5Provider.ComputeHash(tmpIV);

            //將data轉成utf8編碼 byte array
            byte[] byteData = Encoding.UTF8.GetBytes(data);

            //加密
            RijndaelManaged aesProvider = new RijndaelManaged();
            ICryptoTransform aesEncrypt = aesProvider.CreateEncryptor(key, iv);
            byte[] result = aesEncrypt.TransformFinalBlock(byteData, 0, byteData.Length);

            //轉成base64字串
            string base64Result = Convert.ToBase64String(result);
            return base64Result;
        }

        //解密
        private static string decrypt(string data, string strKey, string strIV)
        {
            //將key轉成utf8編碼 byte array
            byte[] tmpkey = System.Text.Encoding.UTF8.GetBytes(strKey);

            //將iv轉成utf8編碼 byte array
            byte[] tmpIV = System.Text.Encoding.UTF8.GetBytes(strIV);

            MD5CryptoServiceProvider mD5Provider = new MD5CryptoServiceProvider();

            //將tempkey取得md5的hash,結果會是16 byte的array            
            byte[] key = mD5Provider.ComputeHash(tmpkey);

            //將tmpIV取得md5的hash,結果會是16 byte的array  
            byte[] iv = mD5Provider.ComputeHash(tmpIV);

            //將base64字串轉成byte array
            byte[] encryptData = Convert.FromBase64String(data);

            //解密
            RijndaelManaged aesProvider = new RijndaelManaged();
            ICryptoTransform aesDecrypt = aesProvider.CreateDecryptor(key, iv);
            byte[] result = aesDecrypt.TransformFinalBlock(encryptData, 0, encryptData.Length);


            //將解密後的內容還原成utf8編碼的字串

            string pText = Encoding.UTF8.GetString(result);

            return pText;

        }
        private static string ByteToHex(byte[] ba)
        {
            return "[" + BitConverter.ToString(ba).Replace("-", ",") + "]";
        }
    }

}