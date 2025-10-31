using System.Security.Cryptography;
using System.Text;

namespace StudentListAPI
{
    public static class Utility
    {

        public static string GenerateHashfor128Bit(string stringToHash)
        {
            // Convert input string to bytes
            byte[] data = Encoding.UTF8.GetBytes(stringToHash);

            // Initialize four 32-bit hash values with arbitrary primes
            uint h1 = 0xA5A5A5A5;
            uint h2 = 0x5A5A5A5A;
            uint h3 = 0x3C3C3C3C;
            uint h4 = 0xC3C3C3C3;

            foreach (byte b in data)
            {
                // Mix each byte into the hash values
                h1 ^= b;
                h2 += RotateLeft(h1, 5) ^ b;
                h3 ^= RotateLeft(h2, 7) + b;
                h4 += RotateLeft(h3, 11) ^ b;

                // Optional: further scramble
                h1 = RotateLeft(h1, 13) + h4;
                h2 = RotateLeft(h2, 17) ^ h1;
                h3 = RotateLeft(h3, 19) + h2;
                h4 = RotateLeft(h4, 23) ^ h3;
            }

            // Combine all four hash values into a 16-byte array (128-bit)
            byte[] hash = new byte[16];
            Array.Copy(BitConverter.GetBytes(h1), 0, hash, 0, 4);
            Array.Copy(BitConverter.GetBytes(h2), 0, hash, 4, 4);
            Array.Copy(BitConverter.GetBytes(h3), 0, hash, 8, 4);
            Array.Copy(BitConverter.GetBytes(h4), 0, hash, 12, 4);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2")); // "x2" → lowercase hex; use "X2" for uppercase
            }
            return sb.ToString();
        }

        static uint RotateLeft(uint value, int bits)
        {
            return (value << bits) | (value >> (32 - bits));
        }
    }
}
