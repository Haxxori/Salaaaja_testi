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
    class TripleDESencryption // Compileri ei tunnista classia tällä hetkellä, enkä ole löytänyt ratkaisua miksi tätä classia ei voi referoida
    {
        public byte[] Encrypt(string Text, byte[] Key, byte[] IV)
        {
            if (Text == null || Text.Length <= 0) throw new ArgumentException("Text");
            if (Key == null || Key.Length <= 0) throw new ArgumentException("Key");
            if (IV == null || IV.Length <= 0) throw new ArgumentException("IV");

            byte[] eData;

            //TripleDES object
            using (TripleDES tripledes = TripleDES.Create())
            {
                tripledes.Key = Key;
                tripledes.IV = IV;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, tripledes.CreateEncryptor(tripledes.Key, tripledes.IV), CryptoStreamMode.Write))
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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
