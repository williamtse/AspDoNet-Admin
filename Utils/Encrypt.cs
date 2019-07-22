using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
delegate string GetHash(string password, byte[] salt);

namespace Admin.Utils
{
    public class Encrypt
    {
        public static byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        public static string GetHash(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
        }
            

        public static HashPair Password(string password)
        {
            byte[] salt = Encrypt.GetSalt();
            return new HashPair{ Hashed= Encrypt.GetHash(password, salt),
                Salt = Convert.ToBase64String(salt) };
        }

        public static bool Check(string password, User user)
        {
            byte[] salt = Convert.FromBase64String(user.Salt); ;

            string hashed = Encrypt.GetHash(password, salt);

            return hashed == user.Password;
        }
    }
}
