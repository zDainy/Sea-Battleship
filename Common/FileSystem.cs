using System;
using System.IO;
using System.Text;

namespace Common
{
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

        private static byte GetHash(byte[] input, int type)
        {
            if (input.Length == 2)
            {
                switch (type)
                {
                    case (1):
                        return (byte)(input[0] & input[1]);
                    case (2):
                        return (byte)(input[0] | input[1]);
                    case (3):
                        return (byte)(input[0] ^ input[1]);
                    case (4):
                        return (byte)(input[0] + (input[1] >> 1));
                }
            }
            if (input.Length == 3)
            {
                switch (type)
                {
                    case (1):
                        return (byte)(input[0] & input[1] ^ input[2]);
                    case (2):
                        return (byte)(input[0] | input[1] ^ input[2]);
                    case (3):
                        return (byte)(input[0] ^ input[1] & input[2]);
                    case (4):
                        return (byte)(input[0] ^ input[1] | input[2]);
                    case (5):
                        return (byte)(input[0] & input[1] | input[2]);
                    case (6):
                        return (byte)(input[0] ^ input[1] ^ input[2]);
                }
            }
            if (input.Length == 5)
            {
                byte[] first = new byte[3] { input[0], input[1], input[2] };
                byte[] second = new byte[3] { 0, input[3], input[4] };
                switch (type)
                {
                    case (1):
                        second[0] = GetHash(first, 1);
                        return GetHash(second, 3);
                    case (2):
                        second[0] = GetHash(first, 2);
                        return GetHash(second, 6);
                    case (3):
                        second[0] = GetHash(first, 2);
                        return GetHash(second, 4);
                    case (4):
                        second[0] = GetHash(first, 1);
                        return GetHash(second, 1);
                    case (5):
                        second[0] = GetHash(first, 4);
                        return GetHash(second, 4);
                    case (6):
                        second[0] = GetHash(first, 5);
                        return GetHash(second, 2);
                    case (7):
                        second[0] = GetHash(first, 3);
                        return GetHash(second, 4);
                    case (8):
                        second[0] = GetHash(first, 3);
                        return GetHash(second, 5);
                    case (9):
                        second[0] = GetHash(first, 1);
                        return GetHash(second, 5);
                    case (10):
                        second[0] = GetHash(first, 6);
                        return GetHash(second, 3);
                }
            }
            if (input.Length == 8)
            {
                byte[] first = new byte[3] { input[0], input[1], input[2] };
                byte[] second = new byte[3] { input[3], input[4], input[5] };
                byte[] third = new byte[2] { input[6], input[7] };
                switch (type)
                {
                    case (1):
                        byte[] res1 = new byte[3] { GetHash(first, 1), GetHash(second, 3), GetHash(third, 2) };
                        return GetHash(res1, 4);
                    case (2):
                        byte[] res2 = new byte[3] { GetHash(first, 2), GetHash(second, 6), GetHash(third, 3) };
                        return GetHash(res2, 5);
                    case (3):
                        byte[] res3 = new byte[3] { GetHash(first, 2), GetHash(second, 4), GetHash(third, 3) };
                        return GetHash(res3, 1);
                    case (4):
                        byte[] res4 = new byte[3] { GetHash(first, 1), GetHash(second, 3), GetHash(third, 4) };
                        return GetHash(res4, 2);
                    case (5):
                        byte[] res5 = new byte[3] { GetHash(first, 4), GetHash(second, 2), GetHash(third, 3) };
                        return GetHash(res5, 1);
                    case (6):
                        byte[] res6 = new byte[3] { GetHash(first, 5), GetHash(second, 2), GetHash(third, 1) };
                        return GetHash(res6, 3);
                    case (7):
                        byte[] res7 = new byte[3] { GetHash(first, 3), GetHash(second, 4), GetHash(third, 2) };
                        return GetHash(res7, 1);
                    case (8):
                        byte[] res8 = new byte[3] { GetHash(first, 3), GetHash(second, 5), GetHash(third, 4) };
                        return GetHash(res8, 6);
                    case (9):
                        byte[] res9 = new byte[3] { GetHash(first, 1), GetHash(second, 5), GetHash(third, 3) };
                        return GetHash(res9, 2);
                    case (10):
                        byte[] res10 = new byte[3] { GetHash(first, 6), GetHash(second, 3), GetHash(third, 1) };
                        return GetHash(res10, 2);
                }
            }
            return 0x00;
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
            if (input < 16)
                return '0' + input.ToString("X");
            return input.ToString("X");
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
            byte[] result = new byte[36];
            result[26] = GetHash(new byte[3] { bytes[0], bytes[3], bytes[6] }, 1);
            result[27] = GetHash(new byte[3] { bytes[9], bytes[1], bytes[4] }, 2);
            result[28] = GetHash(new byte[3] { bytes[8], bytes[11], bytes[2] }, 3);
            result[29] = GetHash(new byte[3] { bytes[0], bytes[9], bytes[7] }, 4);
            result[30] = GetHash(new byte[3] { bytes[3], bytes[1], bytes[11] }, 5);
            result[31] = GetHash(new byte[3] { bytes[6], bytes[4], bytes[2] }, 6);
            result[32] = GetHash(new byte[2] { bytes[5], bytes[7] }, 1);
            result[33] = GetHash(new byte[2] { bytes[10], bytes[12] }, 2);
            result[34] = GetHash(new byte[2] { bytes[5], bytes[10] }, 3);
            result[35] = GetHash(new byte[2] { bytes[7], bytes[12] }, 4);
            byte[] keys = new byte[13];
            random.NextBytes(keys);
            for (int i = 0; i < 13; i++)
            {
                result[2 * i] = Vigenere(bytes[i], keys[i]);
                result[2 * i + 1] = keys[i];
            }
            string res = "";
            for (int i = 0; i < result.Length; i++)
            {
                res += ByteToHex(result[i]);
            }
            return res;
        }

        public static void SaveArrangement(string name, bool[,]input)
        {
            string s = saveArrangement(input);
            FileStream fileStream = new FileStream(name, FileMode.Create);
            Encoding e = Encoding.ASCII;
            fileStream.Write(e.GetBytes(s), 0, 72);
            fileStream.Close();
        }

        private static bool[,] loadArrangement(string input)
        {
            if (input.Length != 72) throw new LoadingArrangementException();
            byte[] result = new byte[36];
            for (int i = 0; i < 36; i++)
            {
                result[i] = HexToByte(input[2 * i].ToString() + input[2 * i + 1]);
            }
            byte[] bytes = new byte[13];
            for (int i = 0; i < 13; i++)
            {
                bytes[i] = UnVigenere(result[2 * i], result[2 * i + 1]);
            }
            if (result[26] != GetHash(new byte[3] { bytes[0], bytes[3], bytes[6] }, 1)) throw new LoadingArrangementException();
            if (result[27] != GetHash(new byte[3] { bytes[9], bytes[1], bytes[4] }, 2)) throw new LoadingArrangementException();
            if (result[28] != GetHash(new byte[3] { bytes[8], bytes[11], bytes[2] }, 3)) throw new LoadingArrangementException();
            if (result[29] != GetHash(new byte[3] { bytes[0], bytes[9], bytes[7] }, 4)) throw new LoadingArrangementException();
            if (result[30] != GetHash(new byte[3] { bytes[3], bytes[1], bytes[11] }, 5)) throw new LoadingArrangementException();
            if (result[31] != GetHash(new byte[3] { bytes[6], bytes[4], bytes[2] }, 6)) throw new LoadingArrangementException();
            if (result[32] != GetHash(new byte[2] { bytes[5], bytes[7] }, 1)) throw new LoadingArrangementException();
            if (result[33] != GetHash(new byte[2] { bytes[10], bytes[12] }, 2)) throw new LoadingArrangementException();
            if (result[34] != GetHash(new byte[2] { bytes[5], bytes[10] }, 3)) throw new LoadingArrangementException();
            if (result[35] != GetHash(new byte[2] { bytes[7], bytes[12] }, 4)) throw new LoadingArrangementException();
            bool[] scrambled = new bool[104];
            for (int i = 0; i < 13; i++)
            {
                bool[] q;
                q = ByteToBool(bytes[i]);
                for (int j = 0; j < 8; j++)
                {
                    scrambled[8 * i + j] = q[j];
                }
            }
            bool[] field = Unscramble(scrambled, 4);
            bool[,] fin = Bending<bool>(field, 10, 10);
            return fin;
        }

        public static bool[,] LoadArrangement(string name)
        {
            if (!File.Exists(name)) throw new LoadingArrangementException();
            FileStream fileStream = new FileStream(name,FileMode.Open);
            byte[] bytes = new byte[72];
            fileStream.Read(bytes, 0, 72);
            Encoding e = Encoding.ASCII;
            string s = e.GetString(bytes);
            fileStream.Close();
            return loadArrangement(s);
        }
    }
}
