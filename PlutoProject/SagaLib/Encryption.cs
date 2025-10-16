using System;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Math;

namespace SagaLib
{
    public class Encryption
    {
        public static BigInteger Two = BigInteger.Two;

        public static BigInteger Module = new BigInteger(
            "f488fd584e49dbcd20b49de49107366b336c380d451d0f7c88b31c7c5b2d8ef6f3c923c043f0a55b188d8ebb558cb85d38d334fd7c175743a31d186cde33212cb52aff3ce1b1294018118d7c84a70a72d686c40319c807297aca950cd9969fabd00a509b0246d3083d66a45d419f9c7cbd894b221926baaba25ec355e92f78c7");

        private readonly Rijndael aes;

        private BigInteger privateKey = Two;

        public Encryption()
        {
            aes = Rijndael.Create();
            aes.Mode = CipherMode.ECB;
            aes.KeySize = 128;
            aes.Padding = PaddingMode.None;
        }

        public byte[] AESKey { get; set; }

        public bool IsReady => AESKey != null;

        public void MakePrivateKey()
        {
            var sha = SHA1.Create();
            var tmp = new byte[40];
            sha.TransformBlock(
                Encoding.ASCII.GetBytes(DateTime.Now.ToString() + DateTime.Now.ToUniversalTime() +
                                        DateTime.Now.ToLongDateString()), 0, 40, tmp, 0);
            privateKey = new BigInteger(tmp);
        }

        public byte[] GetKeyExchangeBytes()
        {
            if (privateKey == Two)
            {
                return null;
            }

            return Two.ModPow(privateKey, Module).ToByteArray();
        }

        public void MakeAESKey(string keyExchangeBytes)
        {
            var A = new BigInteger(keyExchangeBytes);
            var R = A.ModPow(privateKey, Module).ToByteArray();
            AESKey = new byte[16];
            Array.Copy(R, AESKey, 16);
            for (var i = 0; i < 16; i++)
            {
                var tmp = (byte)(AESKey[i] >> 4);
                var tmp2 = (byte)(AESKey[i] & 0xF);
                tmp = (tmp > 9) ? (byte)(tmp - 9) : tmp;
                tmp2 = (tmp2 > 9) ? (byte)(tmp2 - 9) : tmp2;
                AESKey[i] = (byte)((tmp << 4) | tmp2);
            }
        }

        public byte[] Encrypt(byte[] src, int offset)
        {
            if (AESKey == null)
            {
                return src;
            }

            if (offset == src.Length)
            {
                return src;
            }

            var crypt = aes.CreateEncryptor(AESKey, new byte[16]);
            var len = src.Length - offset;
            var buf = new byte[src.Length];
            src.CopyTo(buf, 0);
            crypt.TransformBlock(src, offset, len, buf, offset);
            return buf;
        }

        public byte[] Decrypt(byte[] src, int offset)
        {
            if (AESKey == null)
            {
                return src;
            }

            if (offset == src.Length)
            {
                return src;
            }

            var crypt = aes.CreateDecryptor(AESKey, new byte[16]);
            var len = src.Length - offset;
            var buf = new byte[src.Length];
            src.CopyTo(buf, 0);
            var offset2 = 0;
            if (len > 1024)
            {
            }

            while (len > 0)
            {
                var length = len >= 16 ? 16 : len;
                var tmp = new byte[length];
                Array.Copy(src, offset + offset2, tmp, 0, length);
                len -= length;
                crypt.TransformBlock(tmp, 0, length, tmp, 0);
                Array.Copy(tmp, 0, buf, offset + offset2, length);
                offset2 += length;
            }

            return buf;
        }
    }
}