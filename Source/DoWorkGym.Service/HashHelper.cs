using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoWorkGym.Service
{
    public static class HashHelper
    {
        public static string GetPasswordSalt()
        {
            return DateTime.Now.ToString("yyyyMMdd.HHmmss");
        }


        /// <summary>
        /// Returns 64 byte hash of password + salt.
        /// </summary>
        /// <param name="password">May not be null or empty string</param>
        /// <param name="salt">May not be null or empty string</param>
        /// <returns></returns>
        public static string GetHash(string password, string salt)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("password", "Argument may not be null or whitespace");
            }
            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new ArgumentNullException("salt", "Argument may not be null or whitespace");
            }

            HashAlgorithm algorithm = new SHA512Managed();

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] passworndAndSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];

            passwordBytes.CopyTo(passworndAndSaltBytes, 0);
            saltBytes.CopyTo(passworndAndSaltBytes, passwordBytes.Length);
            var bytePassword = algorithm.ComputeHash(passworndAndSaltBytes);

            return Encoding.UTF8.GetString(bytePassword);
        }


        /// <summary>
        /// Confirm password that the user tries to login with the stored passsword
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt">Datetime stamp</param>
        /// <param name="hashByteArray">Byte[] converted to string</param>
        /// <returns></returns>
        public static Boolean ConfirmPassword(string password, string salt, string hashByteArray)
        {
            //Convert string to byte[]
            byte[] passwordStored = Encoding.UTF8.GetBytes(hashByteArray);

            //Get complete password string
            var newByteArray = GetHash(password, salt);

            //Convert string to byte[]
            byte[] passwordToCheck = Encoding.UTF8.GetBytes(newByteArray);

            int byteCounter = 0;
            foreach (var bite in passwordToCheck)
            {
                if (bite != passwordStored[byteCounter])
                {
                    //Is not matching
                    return false;
                }
                byteCounter++;
            }

            //Is matching
            return true;
        }
    }
}
