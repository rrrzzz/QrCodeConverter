using System;
using System.Collections.Generic;
using System.Text;

namespace QRConverter
{
    public static class StringExtensions
    {
        public static List<string> Divide(this string text, int size, string encoding)
        {
            var stringsList = new List<string>();
            int startIndex = 0, byteSum = 0;
            int chunks = size % Constants.LMaxByteSize == 0
                ? size / Constants.LMaxByteSize
                : size / Constants.LMaxByteSize + 1;
            for (int i = 0; i < text.Length; i++)
            {
                var charValue = text[i].ToString().CountBytes(encoding);
                byteSum += charValue;
                if (byteSum <= Constants.LMaxByteSize) continue;
                stringsList.Add(text.Substring(startIndex, i - startIndex));
                chunks--;
                if (chunks == 1)
                {
                    stringsList.Add(text.Substring(i));
                    return stringsList;
                }
                startIndex = i;
                byteSum = charValue;
            }
            throw new ArgumentException("Invalid string size specified.");
        }

        public static int CountBytes(this string str, string encoding)
        {
            try
            {
                return Encoding.GetEncoding(encoding).GetByteCount(str);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Invalid encoding specified.");
                throw ex;
            }
        }
    }

    
}
