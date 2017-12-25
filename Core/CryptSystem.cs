using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class CryptSystem
    {
        private static Random random = new Random();
        /// <summary>
        /// Представляет байт в виде строки из двух символов.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ByteToHex(byte input)
        {
            return (input < 16) ? '0' + input.ToString("X") : input.ToString("X");
        }

        /// <summary>
        /// По НЕХ-коду получает байт.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte HexToByte(string input)
        {
            return Convert.ToByte(input, 16);
        }

        /// <summary>
        /// Генерирует битовый ключ указанной длины
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool[] KeyGen(int size)
        {
            bool[] result = new bool[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = random.NextDouble() < 0.5;
            }
            return result;
        }

        public static bool[] Scramble(bool[] input, int keylength)
        {
            bool[] result = new bool[input.Length + keylength];
            bool[] key = KeyGen(keylength);
            for (int i = 0; i < keylength; i++)
            {
                result[input.Length + i] = key[i];
            }
            int j = 0;
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = input[i] ^ key[j++];
                if (j == keylength) j = 0;
            }
            return result;
        }

        public static bool[] Unscramble(bool[] input, int keylength)
        {
            bool[] key = new bool[keylength];
            for (int i = 0; i < keylength; i++)
            {
                key[i] = input[input.Length - keylength + i];
            }
            bool[] result = new bool[input.Length - keylength];
            int j = 0;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = input[i] ^ key[j++];
                if (j == keylength) j = 0;
            }
            return result;
        }

        /// <summary>
        /// Генерирует хеш-сумму в виде байтового массива для байтового массива
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] GetHash(byte[] input)
        {
            int size = (int)Math.Round(Math.Sqrt(input.Length));
            List<byte>[] hash = new List<byte>[size];
            for (int i = 0; i < size; i++)
            {
                hash[i] = new List<byte>();
                hash[i].Add((byte)(input.Length + i));
            }
            byte[] res = new byte[size];
            for (int i = 0; i < input.Length; i++)
            {
                int p;
                Math.DivRem(i, size, out p);
                hash[p].Add(input[i]);
            }
            for (int i = 0; i < size; i++)
            {
                byte b = (byte)hash[i].Count;
                foreach (byte z in hash[i])
                {
                    b = (byte)(b ^ z);
                }
                res[i] = b;
            }
            return res;
        }

        /// <summary>
        /// Проверка хеш-суммы для заданного массива.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool CheckHash(byte[] input, byte[] hash)
        {
            byte[] gethash = GetHash(input);
            if (gethash.Length != hash.Length) return false;
            for (int i = 0; i < hash.Length; i++)
            {
                if (gethash[i] != hash[i]) return false;
            }
            return true;
        }

        public static byte BoolToByte(bool[] input)
        {
            if (input.Length != 8) throw new ArgumentException("Incorrect array size");
            byte result = 0;
            for (int i = 0; i < 8; i++)
            {
                if (input[7 - i]) result += (byte)Math.Pow(2, i);
            }
            return result;
        }

        public static bool[] ByteToBool(byte input)
        {
            bool[] result = new bool[8];
            byte sum = 0;
            for (int i = 7; i >= 0; i--)
            {
                if (Math.Pow(2, i) + sum <= input)
                {
                    result[7 - i] = true;
                    sum += (byte)Math.Pow(2, i);
                }
            }
            return result;
        }

        public static byte Vigenere(byte input, byte key)
        {
            return (byte)(input + key);
        }

        public static byte[] Vigenere(byte[] input, byte key)
        {
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Vigenere(input[i], key);
            }
            return input;
        }

        public static byte[] Vigenere(byte[] input, byte[] key)
        {
            int j = 0;
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Vigenere(input[i], key[j++]);
                if (j == key.Length) j = 0;
            }
            return input;
        }

        public static byte UnVigenere(byte input, byte key)
        {
            return (byte)(input - key);
        }

        public static byte[] UnVigenere(byte[] input, byte key)
        {
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = UnVigenere(input[i], key);
            }
            return input;
        }

        public static byte[] UnVigenere(byte[] input, byte[] key)
        {
            int j = 0;
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = UnVigenere(input[i], key[j++]);
                if (j == key.Length) j = 0;
            }
            return input;
        }
    }
}
