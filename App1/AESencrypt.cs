using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Reflection;
using Windows.Security.Cryptography.Core; //?

namespace App1
{
    class AESencrypt //
    {
        public byte[] Encrypt(string Text, byte[] Key, byte[] IV)
        {
            if (Text == null || Text.Length <= 0) throw new ArgumentException("Text");
            if (Key == null || Key.Length <= 0) throw new ArgumentException("Key");
            if (IV == null || IV.Length <= 0) throw new ArgumentException("IV");

            byte[] eData;

            //Aes object
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(Text);
                        }

                        eData = ms.ToArray();
                    }
                }
            }
            return eData;
        }

    }
}
