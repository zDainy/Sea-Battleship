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

        public static T[] Lining<T>(T[,] input)
        {
            int n = input.GetLength(0);
            int m = input.GetLength(1);
            T[] result = new T[n * m];
            for (int i = 0; i < n; i++)
            {

                for (int j = 0; j < m; j++)
                {
                    result[i * m + j] = input[i, j];
                }
            }
            return result;
        }

        public static T[,] Bending<T>(T[] input, int n, int m)
        {
            T[,] result = new T[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    result[i, j] = input[i * m + j];
                }
            }
            return result;
        }

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

        /// <summary>
        /// Представляет расстановку кораблей в байтовый массив.
        /// </summary>
        /// <param name="arrangement"></param>
        /// <returns></returns>
        public static string ArrangementToByte(ShipArrangement arrangement)
        {
            CellStatе[] tmp = Lining<CellStatе>(arrangement.GetArrangement());
            byte[] result = new byte[25];
            for (int i = 0; i < 25; i++)
            {
                switch (tmp[4 * i])
                {
                    case CellStatе.Water:
                        result[i] = 0;
                        break;
                    case CellStatе.WoundedWater:
                        result[i] = 0x40;
                        break;
                    case CellStatе.Ship:
                        result[i] = 0x80;
                        break;
                    case CellStatе.WoundedShip:
                    case CellStatе.DestroyedShip:
                        result[i] = 0xC0;
                        break;
                }
                switch (tmp[4 * i + 1])
                {
                    case CellStatе.Water:
                        break;
                    case CellStatе.WoundedWater:
                        result[i] += 0x10;
                        break;
                    case CellStatе.Ship:
                        result[i] += 0x20;
                        break;
                    case CellStatе.WoundedShip:
                    case CellStatе.DestroyedShip:
                        result[i] += 0x30;
                        break;
                }
                switch (tmp[4 * i + 2])
                {
                    case CellStatе.Water:
                        break;
                    case CellStatе.WoundedWater:
                        result[i] += 0x04;
                        break;
                    case CellStatе.Ship:
                        result[i] += 0x08;
                        break;
                    case CellStatе.WoundedShip:
                    case CellStatе.DestroyedShip:
                        result[i] += 0x0C;
                        break;
                }
                switch (tmp[4 * i + 3])
                {
                    case CellStatе.Water:
                        break;
                    case CellStatе.WoundedWater:
                        result[i] += 0x01;
                        break;
                    case CellStatе.Ship:
                        result[i] += 0x02;
                        break;
                    case CellStatе.WoundedShip:
                    case CellStatе.DestroyedShip:
                        result[i] += 0x03;
                        break;
                }
            }
            string res = "";
            for (int i =0;i<25;i++)
            {
                res += ByteToHex(result[i]);
            }
            return res;
        }

        public static ShipArrangement ByteToArrangement(string str)
        {
            byte[] input = new byte[25];
            for (int i=0;i<25;i++)
            {
                input[i] = HexToByte(str[2 * i].ToString() + str[2 * i + 1]);
            }
            ShipArrangement result = new ShipArrangement();
            CellStatе[] map = new CellStatе[100];
            for (int i = 0; i < 25; i++)
            {
                bool[] tmp = CryptSystem.ByteToBool(input[i]);
                if (tmp[0])
                {
                    if (tmp[1]) map[4*i]=CellStatе.WoundedShip;
                    else map[4*i] = CellStatе.Ship;
                }
                else
                {
                    if (tmp[1]) map[4*i] = CellStatе.WoundedWater;
                    else map[4*i] = CellStatе.Water;
                }
                if (tmp[2])
                {
                    if (tmp[3]) map[4*i+1] = CellStatе.WoundedShip;
                    else map[4*i+1] = CellStatе.Ship;
                }
                else
                {
                    if (tmp[3]) map[4*i+1] = CellStatе.WoundedWater;
                    else map[4*i+1] =  CellStatе.Water;
                }
                if (tmp[4])
                {
                    if (tmp[5]) map[4*i+2] = CellStatе.WoundedShip;
                    else map[4*i+2] = CellStatе.Ship;
                }
                else
                {
                    if (tmp[5]) map[4*i+2] = CellStatе.WoundedWater;
                    else map[4*i+2] = CellStatе.Water;
                }
                if (tmp[6])
                {
                    if (tmp[7]) map[4*i+3] = CellStatе.WoundedShip;
                    else map[4*i+3] = CellStatе.Ship;
                }
                else
                {
                    if (tmp[7]) map[4*i+3] = CellStatе.WoundedWater;
                    else map[4*i+3] = CellStatе.Water;
                }
            }
            CellStatе[,] a = Bending<CellStatе>(map, 10, 10);
            for (int i=0;i<10;i++)
            {
                for (int j = 0; j < 10; j++)
                    result.SetCellState(a[i, j], i, j);
            }
            return result;
        }
    }
}
