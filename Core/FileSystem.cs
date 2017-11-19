using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class FileSystem
    {
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
                Random r = new Random(DateTime.Now.Millisecond ^ DateTime.Now.Second);
                result[i] = r.NextDouble() < 0.5;
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

        public static byte GetHash(byte[] input, int type)
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
    }
}
