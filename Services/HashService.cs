using System;
using System.Security.Cryptography;
using System.Text;
using MyPrimerWebApi.Models;

namespace MyPrimerWebApi.Services
{
    public class HashService : IHashService
    {
        public HashResult HashData(string input)
        {
            // generate a random salt
            var saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            var saltText = Convert.ToBase64String(saltBytes);
            return HashData(input, saltText);
        }

        public HashResult HashData(string input, string salt)
        {
            string hashedData = "";

            using (var sha = SHA256.Create())
            {
                string saltedPassword = input + salt;
                hashedData = Convert.ToBase64String(
                  sha.ComputeHash(Encoding.Unicode.GetBytes(
                  saltedPassword)));
            }
            return new HashResult()
            {
                Hash = hashedData,
                Salt = salt
            };
        }
    }
}