using Core;
using Core.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    /// <summary>
    /// Подсистема работы с файлами игры.
    /// </summary>
    public static class FileSystem
    {
        private static Random random = new Random();

        private static T[] Lining<T>(T[,] input)
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

        private static byte[] Vigenere(byte[] input, byte key)
        {
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Vigenere(input[i], key);
            }
            return input;
        }

        private static byte[] Vigenere(byte[] input, byte[] key)
        {
            int j = 0;
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = Vigenere(input[i], key[j++]);
                if (j == key.Length) j = 0;
            }
            return input;
        }

        private static byte UnVigenere(byte input, byte key)
        {
            return (byte)(input - key);
        }

        private static byte[] UnVigenere(byte[] input, byte key)
        {
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = UnVigenere(input[i], key);
            }
            return input;
        }

        private static byte[] UnVigenere(byte[] input, byte[] key)
        {
            int j = 0;
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = UnVigenere(input[i], key[j++]);
                if (j == key.Length) j = 0;
            }
            return input;
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
        /// 
        public static void SaveArrangement(string name, ShipArrangement input)
        {
            bool[,] arrangement = new bool[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
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
            if (input.Length != 68) throw new LoadingArrangementException();
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
        /// 
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
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    res.SetCellState((arrangement[i, j]) ? CellStatе.Ship : CellStatе.Water, i, j);
                }
            }
            return res;
        }

        private static byte[,] ArrangementsToByteArray(ShipArrangement a, ShipArrangement b) // да, я знаю, что этот код не самый очевидный. Рекомендую просто игнорировать его существование
        {
            byte[,] result = new byte[5, 10];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    switch (a.GetCellState(2 * i, j))
                    {
                        case CellStatе.Water:
                            result[i, j] = 0;
                            break;
                        case CellStatе.WoundedWater:
                            result[i, j] = 0x40;
                            break;
                        case CellStatе.Ship:
                            result[i, j] = 0x80;
                            break;
                        case CellStatе.WoundedShip:
                        case CellStatе.DestroyedShip:
                            result[i, j] = 0xC0;
                            break;
                    }
                    switch (a.GetCellState(2 * i + 1, j))
                    {
                        case CellStatе.Water:
                            break;
                        case CellStatе.WoundedWater:
                            result[i, j] += 0x10;
                            break;
                        case CellStatе.Ship:
                            result[i, j] += 0x20;
                            break;
                        case CellStatе.WoundedShip:
                        case CellStatе.DestroyedShip:
                            result[i, j] += 0x30;
                            break;
                    }
                    switch (b.GetCellState(2 * i, j))
                    {
                        case CellStatе.Water:
                            break;
                        case CellStatе.WoundedWater:
                            result[i, j] += 0x04;
                            break;
                        case CellStatе.Ship:
                            result[i, j] += 0x08;
                            break;
                        case CellStatе.WoundedShip:
                        case CellStatе.DestroyedShip:
                            result[i, j] += 0x0C;
                            break;
                    }
                    switch (b.GetCellState(2 * i + 1, j))
                    {
                        case CellStatе.Water:
                            break;
                        case CellStatе.WoundedWater:
                            result[i, j] += 0x01;
                            break;
                        case CellStatе.Ship:
                            result[i, j] += 0x02;
                            break;
                        case CellStatе.WoundedShip:
                        case CellStatе.DestroyedShip:
                            result[i, j] += 0x03;
                            break;
                    }
                }
            }
            return result;
        }

        private static ShipArrangement[] ByteArrayToArrangements(byte[,] map) // см. комментарий к предыдущему методу
        {
            ShipArrangement[] result = new ShipArrangement[2];
            result[0] = new ShipArrangement();
            result[1] = new ShipArrangement();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    bool[] tmp = ByteToBool(map[i, j]);
                    if (tmp[0])
                    {
                        if (tmp[1]) result[0].SetCellState(CellStatе.WoundedShip, 2 * i, j);
                        else result[0].SetCellState(CellStatе.Ship, 2 * i, j);
                    }
                    else
                    {
                        if (tmp[1]) result[0].SetCellState(CellStatе.WoundedWater, 2 * i, j);
                        else result[0].SetCellState(CellStatе.Water, 2 * i, j);
                    }
                    if (tmp[2])
                    {
                        if (tmp[3]) result[0].SetCellState(CellStatе.WoundedShip, 2 * i + 1, j);
                        else result[0].SetCellState(CellStatе.Ship, 2 * i + 1, j);
                    }
                    else
                    {
                        if (tmp[3]) result[0].SetCellState(CellStatе.WoundedWater, 2 * i + 1, j);
                        else result[0].SetCellState(CellStatе.Water, 2 * i + 1, j);
                    }
                    if (tmp[4])
                    {
                        if (tmp[5]) result[1].SetCellState(CellStatе.WoundedShip, 2 * i, j);
                        else result[1].SetCellState(CellStatе.Ship, 2 * i, j);
                    }
                    else
                    {
                        if (tmp[5]) result[1].SetCellState(CellStatе.WoundedWater, 2 * i, j);
                        else result[1].SetCellState(CellStatе.Water, 2 * i, j);
                    }
                    if (tmp[6])
                    {
                        if (tmp[7]) result[1].SetCellState(CellStatе.WoundedShip, 2 * i + 1, j);
                        else result[1].SetCellState(CellStatе.Ship, 2 * i + 1, j);
                    }
                    else
                    {
                        if (tmp[7]) result[1].SetCellState(CellStatе.WoundedWater, 2 * i + 1, j);
                        else result[1].SetCellState(CellStatе.Water, 2 * i + 1, j);
                    }
                }
            }
            return result;
        }

        private static string saveGame(Game g)
        {
            byte[,] result = new byte[10, 10];
            byte[,] map = ArrangementsToByteArray(g.ServerShipArrangement, g.ClientShipArrangement);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    result[j, i] = map[j, i];
                }
            }
            byte[] bytes = new byte[1];
            random.NextBytes(bytes);
            byte config = bytes[0];
            bool[] conf = ByteToBool(config);
            conf[0] = g.GameConfig.IsOnline;
            if (g.GameConfig.IsOnline)  // Сохранение онлайн игры
            {
                byte[] rand = new byte[3];
                random.NextBytes(rand);
                string[] ip = g.GameConfig.Connection.Split(new char[2] { '.', ':' });
                result[5, 0] = byte.Parse(ip[0]);
                result[5, 1] = byte.Parse(ip[1]);
                result[5, 2] = byte.Parse(ip[2]);
                result[5, 3] = byte.Parse(ip[3]);
                byte[] port = BitConverter.GetBytes(int.Parse(ip[4]));
                result[5, 4] = port[0];
                result[5, 5] = port[1];
                conf[0] = g.GetTurnOwner() == PlayerRole.Server;
                conf[3] = (g.GameConfig.GameSpeed == GameSpeed.Slow) || (g.GameConfig.GameSpeed == GameSpeed.Turtle);
                conf[4] = (g.GameConfig.GameSpeed == GameSpeed.Medium) || (g.GameConfig.GameSpeed == GameSpeed.Turtle);
                /*switch (g.GameConfig.GameSpeed)
                {
                    case GameSpeed.Fast:
                        conf[3] = false;
                        conf[4] = false;
                        break;
                    case GameSpeed.Medium:
                        conf[3] = false;
                        conf[4] = true;
                        break;
                    case GameSpeed.Slow:
                        conf[3] = true;
                        conf[4] = false;
                        break;
                    case GameSpeed.Turtle:
                        conf[3] = true;
                        conf[4] = true;
                        break;
                }*/
                config = BoolToByte(conf);
                result[5, 6] = config;
                result[5, 7] = rand[0];
                result[5, 8] = rand[1];
                result[5, 9] = rand[2];
            }
            else                        // Сохранениe оффлайн игры
            {
                byte[] rand = new byte[8];
                random.NextBytes(rand);
                result[5, 0] = rand[7];
                result[5, 1] = rand[0];
                result[5, 2] = rand[1];
                switch (g.GameConfig.BotLvl)
                {
                    case BotLevels.Easy:
                        result[5, 3] = 1;
                        break;
                    case BotLevels.Medium:
                        result[5, 3] = 2;
                        break;
                    case BotLevels.Hard:
                        result[5, 3] = 3;
                        break;
                }
                result[5, 4] = rand[2];
                result[5, 5] = rand[3];
                conf[3] = (g.GameConfig.GameSpeed == GameSpeed.Slow) || (g.GameConfig.GameSpeed == GameSpeed.Turtle);
                conf[4] = (g.GameConfig.GameSpeed == GameSpeed.Medium) || (g.GameConfig.GameSpeed == GameSpeed.Turtle);
                /*switch (g.GameConfig.GameSpeed)
                {
                    case GameSpeed.Fast:
                        conf[3] = false;
                        conf[4] = false;
                        break;
                    case GameSpeed.Medium:
                        conf[3] = false;
                        conf[4] = true;
                        break;
                    case GameSpeed.Slow:
                        conf[3] = true;
                        conf[4] = false;
                        break;
                    case GameSpeed.Turtle:
                        conf[3] = true;
                        conf[4] = true;
                        break;
                }*/
                config = BoolToByte(conf);
                result[5, 6] = config;
                result[5, 7] = rand[4];
                result[5, 8] = rand[5];
                result[5, 9] = rand[6];
            }
            for (int i = 0; i < 10; i++)
            {
                byte[] tmp = new byte[5];
                for (int j = 0; j < 5; j++)
                {
                    tmp[j] = result[j, i];
                }
                byte[] hash = GetHash(tmp);
                result[6, i] = hash[0];
                result[7, i] = hash[1];
                tmp = Vigenere(tmp, hash);
                for (int j = 0; j < 5; j++)
                {
                    result[j, i] = tmp[j];
                }
                tmp = new byte[6];
                for (int j = 0; j < 6; j++)
                {
                    tmp[j] = result[j, i];
                }
                hash = GetHash(tmp);
                result[8, i] = hash[0];
                result[9, i] = hash[1];
            }
            bytes = Lining<byte>(result);
            string res = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                res += ByteToHex(bytes[i]);
            }
            return res;
        }

        /// <summary>
        /// Сохраняет игру и ее настройки в указанный файл. Если файл существует, он будет перезаписан
        /// </summary>
        /// <param name="name">Имя файла с указанным расширением.</param>
        /// <param name="input">Текущая игра.</param>
        public static void SaveGame(string name, Game input)
        {
            string s = saveGame(input);
            FileStream fileStream = new FileStream(name, FileMode.Create);
            Encoding e = Encoding.ASCII;
            fileStream.Write(e.GetBytes(s), 0, e.GetBytes(s).Length);
            fileStream.Close();
        }

        private static Game loadGame(string input)
        {
            if (input.Length != 200) throw new GameLoadingException();
            byte[] bytes = new byte[input.Length / 2];
            for (int i = 0; i < input.Length / 2; i++)
            {
                bytes[i] = HexToByte(input[2 * i].ToString() + input[2 * i + 1]);
            }
            byte[,] result = new byte[10, 10];
            result = FileSystem.Bending<byte>(bytes, 10, 10);
            for (int i = 0; i < 10; i++)
            {
                byte[] tmp = new byte[6];
                for (int j = 0; j < 6; j++)
                {
                    tmp[j] = result[j, i];
                }
                byte[] hash = new byte[2] { result[8, i], result[9, i] };
                if (!CheckHash(tmp, hash)) throw new GameLoadingException();
                hash = new byte[2] { result[6, i], result[7, i] };
                tmp = new byte[5];
                for (int j = 0; j < 5; j++)
                {
                    tmp[j] = result[j, i];
                }
                tmp = UnVigenere(tmp, hash);
                if (!CheckHash(tmp, hash)) throw new GameLoadingException();
                for (int j = 0; j < 5; j++)
                {
                    result[j, i] = tmp[j];
                }
            }
            GameConfig gc;
            PlayerRole to;
            if (!ByteToBool(result[5, 6])[0])
            {
                BotLevels bl = BotLevels.Easy;
                GameSpeed gs;
                switch (result[5, 3])
                {
                    case 1:
                        bl = BotLevels.Easy;
                        break;
                    case 2:
                        bl = BotLevels.Medium;
                        break;
                    case 3:
                        bl = BotLevels.Hard;
                        break;
                }
                bool[] config = ByteToBool(result[5, 6]);
                gs = config[3] ? config[4] ? GameSpeed.Turtle : GameSpeed.Slow : config[4] ? GameSpeed.Medium : GameSpeed.Fast;
                to = config[1] ? PlayerRole.Client : PlayerRole.Server;
                gc = new GameConfig(bl, gs, GameStatus.Pause);
            }
            else
            {
                GameSpeed gs;
                byte[] ip = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    ip[i] = result[5, i];
                }
                int port = BitConverter.ToInt32(new byte[4] { result[5, 4], result[5, 5], 0, 0 }, 0);
                string connection = ip[0].ToString() + '.' + ip[1].ToString() + '.' + ip[2].ToString() + '.' + ip[3].ToString() + ":" + port.ToString();
                bool[] config = ByteToBool(result[5, 6]);
                gs = config[3] ? config[4] ? GameSpeed.Turtle : GameSpeed.Slow : config[4] ? GameSpeed.Medium : GameSpeed.Fast;
                gc = new GameConfig(PlayerRole.Server, connection, gs, GameStatus.Pause);
                to = config[1] ? PlayerRole.Client : PlayerRole.Server;
            }
            byte[,] map = new byte[5, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    map[j, i] = result[j, i];
                }
            }
            ShipArrangement[] arrangements = ByteArrayToArrangements(map);
            Game res = new Game(arrangements[0], arrangements[1], gc, to);
            return res;
        }

        /// <summary>
        /// Загружает игру из указанного файла.
        /// </summary>
        public static Game LoadGame(string name)
        {
            if (!File.Exists(name)) throw new GameLoadingException();
            FileStream fileStream = new FileStream(name, FileMode.Open);
            byte[] bytes = new byte[200];
            fileStream.Read(bytes, 0, bytes.Length);
            Encoding e = Encoding.ASCII;
            string s = e.GetString(bytes);
            fileStream.Close();
            return loadGame(s);
        }
    }
}
