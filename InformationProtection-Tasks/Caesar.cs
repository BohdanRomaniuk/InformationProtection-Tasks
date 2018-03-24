using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace InformationProtection_Tasks
{
    public enum Direction { Left, Right }

    class Cipher
    {
        private Dictionary<char, int> Alphabet { get; set; }
        public int Shift { get; set; }
        public Direction Dir { get; set; }
        public string Message { get; set; }

        public Cipher()
        {
            GenerateAlphabet();
        }

        public Cipher(int _shift, Direction _direction, string _message)
        {
            GenerateAlphabet();
            Shift = _shift;
            Dir = _direction;
            Message = _message;
        }

        private void GenerateAlphabet()
        {
            Alphabet = new Dictionary<char, int>();
            Random indexRandomizer = new Random();
            List<int> randomNumbers = Enumerable.Range(0, 52).ToList();
            for(int i=65; i<=122; ++i)
            {
                if(!(i>90 && i<97))
                {
                    int symbolIndex = indexRandomizer.Next(0, randomNumbers.Count);
                    Alphabet.Add((char)i, randomNumbers[symbolIndex]);
                    randomNumbers.RemoveAt(symbolIndex);
                }
            }
        }

        public char ShiftChar(char symbol, int shift, Direction dir)
        {
            char start = char.IsUpper(symbol) ? 'A' : 'a';
            return (dir == Direction.Right) ? (char)((((symbol + shift) - start) % 26) + start) : (char)((26+((symbol - shift) - start))%26 + start);
        }

        public string CaesarEncode(string message)
        {
            string encoded = String.Empty;
            foreach(char symbol in message)
            {
                encoded += (symbol == ' ') ? ' ' : ShiftChar(symbol, Shift, Dir);
            }
            return encoded;
        }

        public string CaesarDecode(string encoded)
        {
            string decoded = String.Empty;
            foreach(char symbol in encoded)
            {
                decoded += (symbol == ' ') ? ' ' : ShiftChar(symbol, Shift, (Dir==Direction.Right) ? Direction.Left : Direction.Right);
            }
            return decoded;
        }

        public string SimpleReplaceEncode(string message)
        {
            string encoded = String.Empty;
            for(int i=0; i<message.Length; ++i)
            {
                encoded += (message[i] == ' ')?" ":(Alphabet[message[i]].ToString() + " ");
            }
            encoded = encoded.Substring(0, encoded.Length - 1);
            return encoded;
        }

        public string SimpleReplaceDecode(string encoded)
        {
            string decoded = String.Empty;
            string[] words = encoded.Split(new[] { "  " }, StringSplitOptions.None);
            for(int i=0; i<words.Length; ++i)
            {
                string[] chars = words[i].Split(' ');
                for(int j=0; j<chars.Length; ++j)
                {
                    decoded += Alphabet.FirstOrDefault(x => x.Value == Convert.ToInt32(chars[j])).Key;
                }
                decoded += " ";
            }
            return decoded;
        }

        public string Encode()
        {
            return SimpleReplaceEncode(CaesarEncode(Message));
        }

        public string Decode()
        {
            return CaesarDecode(SimpleReplaceDecode(Message));
        }

        public void ReadMessageFromFile(string fileName)
        {
            using (StreamReader reader = new StreamReader("../../" + fileName))
            {
                Message = reader.ReadLine();
            }
        }

        public void WriteDecodedToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter("../../" + fileName))
            {
                writer.WriteLine(Decode());
            }
        }

        public void WriteEncodedToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter("../../" + fileName))
            {
                writer.WriteLine("{0} {1}", Shift, (Dir == Direction.Left) ? "Left" : "Right");
                string alphabetLine = String.Empty;
                foreach(var elem in Alphabet)
                {
                    alphabetLine += String.Format("{0} {1}  ", elem.Key, elem.Value);
                }
                alphabetLine = alphabetLine.Substring(0, alphabetLine.Length - 2);
                writer.WriteLine(alphabetLine);
                writer.Write(Encode());
            }
        }

        public void ReadEncodedFromFile(string fileName)
        {
            using (StreamReader reader = new StreamReader("../../" + fileName))
            {
                string[] shiftAndDirLine = reader.ReadLine().Split(' ');
                Shift = Convert.ToInt32(shiftAndDirLine[0]);
                Dir = (shiftAndDirLine[1] == "Left") ? Direction.Left : Direction.Right;
                string[] alphas = reader.ReadLine().Split(new[] { "  " }, StringSplitOptions.None);
                Alphabet = new Dictionary<char, int>();
                foreach(string pair in alphas)
                {
                    string[] keyAndValue = pair.Split(' ');
                    Alphabet.Add(Convert.ToChar(keyAndValue[0]), Convert.ToInt32(keyAndValue[1]));
                }
                Message = reader.ReadLine();
            }
        }
    }

    class Caesar
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Encoding######");
            Cipher cipher = new Cipher();
            cipher.ReadMessageFromFile("InputMessage.txt");
            Console.WriteLine("Input shift: ");
            cipher.Shift = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Input direction(left or right):");
            cipher.Dir = (Console.ReadLine() == "left") ? Direction.Left : Direction.Right;
            cipher.WriteEncodedToFile("Encoded.txt");
            Console.WriteLine("Input message: " + cipher.Message);
            Console.WriteLine("Caesar: " + cipher.CaesarEncode(cipher.Message));
            Console.WriteLine("Caesar + Simple Replace: " + cipher.Encode());
            Console.WriteLine("Message, table, shift, direction saved to file Encoded.txt");
            Console.ReadKey();
            Console.WriteLine("\nDecoding######");
            cipher.ReadEncodedFromFile("Encoded.txt");
            Console.WriteLine("Decoded message: " + cipher.Decode());
            cipher.WriteDecodedToFile("Decoded.txt");
            Console.WriteLine("Decoded message saved to file Decoded.txt");
            Console.ReadKey();
        }
    }
}
