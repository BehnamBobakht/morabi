using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Base
{
    public class BaseSecurity
    {
        private static string ENCRYPT_KEY = "CODEPROCESS.IR";

        public static string HashMd5(string input)
        {
            if (input == null)
                return "";
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static string ComputeSHA256(string s)
        {
            string hash = String.Empty;

            // Initialize a SHA256 hash object
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash of the given string
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));

                // Convert the byte array to string format
                foreach (byte b in hashValue)
                {
                    hash += $"{b:X2}";
                }
            }

            return hash;
        }
        public static string HashPassword(Guid userUniqueId, string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] tmpSource;
            byte[] tmpHash;

            tmpSource = ASCIIEncoding.ASCII.GetBytes(password + userUniqueId.ToString() + "MASOUD!");
            tmpHash = md5.ComputeHash(tmpSource);

            StringBuilder sOuput = new StringBuilder(tmpHash.Length);
            for (int i = 0; i < tmpHash.Length; i++)
            {
                sOuput.Append(tmpHash[i].ToString("X2"));
            }
            return sOuput.ToString();
        }

        public static string EncryptText(string Input)
        {
            if (Input != null)
            {
                byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(Input);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(ENCRYPT_KEY);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);
                string result = Convert.ToBase64String(bytesEncrypted);
                return result;
            }
            return null;
        }

        public static string DecryptText(string input)
        {
            if (string.IsNullOrEmpty(input) == false)
            {
                byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(ENCRYPT_KEY);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
                byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);
                string result = Encoding.UTF8.GetString(bytesDecrypted);
                return result;
            }
            return null;
        }

        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static bool IsValidInput(string Input, Enum_Validation type)
        {
            Input = Input == null ? Input : Input.Trim();
            if (type != Enum_Validation.NONE)
            {
                if (!string.IsNullOrEmpty(Input))
                {
                    string pattern = GetValidationPattern(type);
                    if (!string.IsNullOrEmpty(pattern))
                    {
                        System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(Input.Trim().Replace(" ", ""), pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        if (!match.Success)
                            return false;
                        else
                            return true;
                    }

                }
                return true;
            }
            return true;
        }

        private static string GetValidationPattern(Enum_Validation type)
        {
            switch (type)
            {
                case Enum_Validation.EMAIL:
                    return BaseWebsite.WebsiteSetting.EmailRegix;
                case Enum_Validation.NUMBER:
                    return @"^\d+?$";
                case Enum_Validation.TEXT:
                    return @"^([a-zA-Z]+)$";
                case Enum_Validation.URL:
                    return @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                case Enum_Validation.DATE:
                    return @"";
                case Enum_Validation.PERSIAN:
                    return @"^[\u0600-\u06FF]+$";
                case Enum_Validation.MOBILE:
                    return BaseWebsite.WebsiteSetting.MobileRegix;

                case Enum_Validation.ZipCode:
                    return BaseWebsite.WebsiteSetting.ZipCodeRegix;
                case Enum_Validation.NationalCode:
                   return BaseWebsite.WebsiteSetting.NationalCodeRegix;
                    //return @"^\d{10}$";

                case Enum_Validation.PASSWORD:
                    return BaseWebsite.WebsiteSetting.PasswordRegix;
                case Enum_Validation.USERNAME:
                    return @"^[a-zA-Z0-9_]{3,30}$";
                default:
                    return "";
            }
            //return @"^09[0-9]{9}$";
        }

        public static string GetClientIPAddress()
        {
            string ipaddress;
            ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ipaddress;
        }

        public static string GetSha1(string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;
            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash;
        }
    }
}
