using System;
using System.IO;
using System.Text;

namespace Common
{
    /// <summary>
    /// Подсистема работы с файлами игры.
    /// </summary>
    class FileSystem
    {
        private static Random random = new Random();

        private static T[] Lining<T>(T[,] input)
        {
            int n = input.GetLength(0);
            int m = input.GetLength(1);
            T[] result = new T[n * m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i * m + j] = input[i, j];
                }
            }
            return result;
        }

        private static T[,] Bending<T>(T[] input, int n, int m)
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

        private static bool[] KeyGen(int size)
        {
            bool[] result = new bool[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = random.NextDouble() < 0.5;
            }
            return result;
        }

        private static bool[] Scramble(bool[] input, int keylength)
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

        private static bool[] Unscramble(bool[] input, int keylength)
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

        private static byte[] GetHash(byte[] input)
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

        private static bool CheckHash(byte[] input, byte[] hash)
        {
            byte[] gethash = GetHash(input);
            if (gethash.Length != hash.Length) return false;
            for (int i = 0; i < hash.Length; i++)
            {
                if (gethash[i] != hash[i]) return false;
            }
            return true;
        }

        private static byte BoolToByte(bool[] input)
        {
            if (input.Length != 8) throw new ArgumentException("Incorrect array size");
            byte result = 0;
            for (int i = 0; i < 8; i++)
            {
                if (input[7 - i]) result += (byte)Math.Pow(2, i);
            }
            return result;
        }

        private static bool[] ByteToBool(byte input)
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

        private static string ByteToHex(byte input)
        {
            return (input < 16) ? '0' + input.ToString("X") : input.ToString("X");
        }

        private static byte HexToByte(string input)
        {
            return Convert.ToByte(input, 16);
        }

        private static byte Vigenere(byte input, byte key)
        {
            return (byte)(input + key);
        }

        private static byte UnVigenere(byte input, byte key)
        {
            return (byte)(input - key);
        }

        private static string saveArrangement(bool[,] input)
        {
            bool[] field = Lining<bool>(input);
            bool[] scrambled = Scramble(field, 4);
            byte[] bytes = new byte[13];
            for (int i = 0; i < 13; i++)
            {
                bool[] tmp = new bool[8];
                for (int j = 0; j < 8; j++)
                {
                    tmp[j] = scrambled[8 * i + j];
                }
                bytes[i] = BoolToByte(tmp);
            }
            byte[] result = new byte[34];
            byte[] hash = GetHash(bytes);
            byte[] keys = new byte[13];
            random.NextBytes(keys);
            for (int i = 0; i < 13; i++)
            {
                result[2 * i] = Vigenere(bytes[i], keys[i]);
                result[2 * i + 1] = keys[i];
                bytes[i] = (byte)(result[2 * i] ^ result[2 * i + 1]);
            }
            for (int i = 0; i < hash.Length; i++)
            {
                result[26 + i] = hash[i];
            }
            hash = GetHash(bytes);
            for (int i = 0; i < hash.Length; i++)
            {
                result[26 + hash.Length + i] = hash[i];
            }
            string res = "";
            for (int i = 0; i < result.Length; i++)
            {
                res += ByteToHex(result[i]);
            }
            return res;
        }
        
        /// <summary>
        /// Сохраняет растановку в указанный файл. Если файл существует, он будет перезаписан.
        /// </summary>
        /// <param name="name">Имя файла с указанным расширением.</param>
        /// <param name="input">Расстановка кораблей.</param>
        public static void SaveArrangement(string name, ShipArrangement input)
        {
            bool[,] arrangement = new bool[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j=0;j<10;j++)
                {
                    arrangement[i, j] = input.GetCellState(i, j) == CellStatе.Ship;
                }
            }
            string s = saveArrangement(arrangement);
            FileStream fileStream = new FileStream(name, FileMode.Create);
            Encoding e = Encoding.ASCII;
            fileStream.Write(e.GetBytes(s), 0, e.GetBytes(s).Length);
            fileStream.Close();
        }

        private static bool[,] loadArrangement(string input)
        {
            byte[] byteinput = new byte[input.Length / 2];
            for (int i = 0; i < byteinput.Length; i++)
            {
                byteinput[i] = HexToByte(input[2 * i].ToString() + input[2 * i + 1]);
            }
            byte[] hashres = new byte[4];
            byte[] hash = new byte[4];
            for (int i = 0; i < hash.Length; i++)
            {
                hash[i] = byteinput[26 + i];
                hashres[i] = byteinput[30 + i];
            }
            byte[] bytes = new byte[13];
            for (int i = 0; i < 13; i++)
            {
                bytes[i] = (byte)(byteinput[2 * i] ^ byteinput[2 * i + 1]);
            }
            if (!CheckHash(bytes, hashres)) throw new LoadingArrangementException();
            for (int i = 0; i < 13; i++)
            {
                bytes[i] = UnVigenere(byteinput[2 * i], byteinput[2 * i + 1]);
            }
            if (!CheckHash(bytes, hash)) throw new LoadingArrangementException();
            bool[] scrambled = new bool[104];
            for (int i = 0; i < 13; i++)
            {
                bool[] tmp = ByteToBool(bytes[i]);
                for (int j = 0; j < tmp.Length; j++)
                {
                    scrambled[i * 8 + j] = tmp[j];
                }
            }
            bool[] res = Unscramble(scrambled, 4);
            return Bending<bool>(res, 10, 10);
        }

        /// <summary>
        /// Загружает расстановкy из указанного файла.
        /// </summary>
        /// <param name="name">Имя файла.</param>
        /// <returns></returns>
        public static ShipArrangement LoadArrangement(string name)
        {
            if (!File.Exists(name)) throw new LoadingArrangementException();
            FileStream fileStream = new FileStream(name, FileMode.Open);
            byte[] bytes = new byte[68];
            fileStream.Read(bytes, 0, bytes.Length);
            Encoding e = Encoding.ASCII;
            string s = e.GetString(bytes);
            fileStream.Close();
            ShipArrangement res = new ShipArrangement();
            bool[,] arrangement = loadArrangement(s);
            for (int i=0;i<10;i++)
            {
                for (int j=0;j<10;j++)
                {
                    res.SetCellState((arrangement[i, j]) ? CellStatе.Ship : CellStatе.Water, i, j);
                }
            }
            return res;
        }
    }
}
