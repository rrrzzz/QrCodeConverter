using System;
using System.Text;

namespace QRConverterTests
{
    public static class StringGen
    {
        public static string GetRandomString(int length, string encoding = "utf-8")
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return GetRandomString(chars, length, encoding);
        }

        public static string GetRandomString(string chars, int length, string encoding = "utf-8")
        {
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            byte[] bytes = Encoding.Default.GetBytes(stringChars);
            return Encoding.GetEncoding(encoding).GetString(bytes);
        }
    }
}
