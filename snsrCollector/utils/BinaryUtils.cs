using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snsrCollector.utils
{
    // Набор утилит по работе с байтиками.
    public static class BinaryUtils
    {
        public static string BytesToHexString(byte[] data, int offset, int count)
        {
            StringBuilder hex = new StringBuilder(data.Length * 2);
            
            for (var bitum = offset; bitum < offset + count; bitum++)
                hex.AppendFormat("{0:x2} ", data[bitum]);
            
            return hex.ToString();
        }

        public static byte[] HexStringToBytes(string hexString)
        {
            hexString = hexString.Trim();
            hexString = hexString.Replace(" ", "");

            return Enumerable.Range(0, hexString.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                     .ToArray();
        }
    }
}
