using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AspNet2.AppLib.Tools
{
    public static class RandomGenerator
    {
        static readonly RNGCryptoServiceProvider csp;

        static RandomGenerator()
        {
            csp = new RNGCryptoServiceProvider();
        }

        public static int Next(int minInclusiveValue, int maxExclusiveValue)
        {
            if (minInclusiveValue >= maxExclusiveValue) throw new ArgumentOutOfRangeException("minValue must be lower than maxExclusiveValue");

            long diff = (long)maxExclusiveValue - minInclusiveValue;
            
            long upperBound = uint.MaxValue / diff * diff;

            uint ui;
            
            do
            {
                ui = GetRandomUInt();
            } while (ui >= upperBound);
            
            return (int)(minInclusiveValue + (ui % diff));
        }

        private static uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        private static byte[] GenerateRandomBytes(int bytesNumber)
        {
            byte[] buffer = new byte[bytesNumber];
            csp.GetBytes(buffer);
            return buffer;
        }
    }
}
