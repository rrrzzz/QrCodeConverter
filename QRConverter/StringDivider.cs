using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRConverter
{
    public static class StringDivider
    {
        public static List<string> DivideString(string text, int size)
        {
            var stringsList = new List<string>();
            var startIndex = 0;
            var byteSum = 0;
            int chunks = size % Constants.LMaxByteSize == 0
                ? size / Constants.LMaxByteSize
                : size / Constants.LMaxByteSize + 1;
            for (int i = 0; i < text.Length - 1; i++)
            {
                byteSum += Encoding.UTF8.GetByteCount(text[i].ToString());
                if (byteSum <= Constants.LMaxByteSize) continue;
                stringsList.Add(text.Substring(startIndex, i - startIndex));
                chunks--;
                if (chunks == 1)
                {
                    stringsList.Add(text.Substring(i));
                    return stringsList;
                }
                startIndex = i;
                byteSum = 0;
            }
            throw new ArgumentException("Invalid string size specified.");
        }
    }
}
