using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace LabRequest.Web.Infrastracture
{
    public class CookieEncryptor
    {
        public static string Encrypt(string plaintextValue)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(plaintextValue);
            return MachineKey.Encode(plaintextBytes, MachineKeyProtection.All);
        }

        public static string Decrypt(string encryptedValue)
        {
            var decryptedBytes = MachineKey.Decode(encryptedValue,
                MachineKeyProtection.All);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}