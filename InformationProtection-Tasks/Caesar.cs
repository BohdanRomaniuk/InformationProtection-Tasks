using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InformationProtection_Tasks
{
    public enum Direction { Left, Right }

    class Cipher
    {
        private Dictionary<char, int> Alphabet { get; set; }
        private int Shift { get; set; }
        private Direction Dir { get; set; }
        private string Message { get; set; }
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

        public char CaesarCharEncode(char symbol, int shift)
        {
            char d = char.IsUpper(symbol) ? 'A' : 'a';
            return (char)((((symbol + shift) - d) % 26) + d);
        }

        public string CaesarEncode()
        {
            string encoded = String.Empty;
            foreach(char symbol in Message)
            {
                encoded += (symbol == ' ') ? ' ' : CaesarCharEncode(symbol, Shift);
            }
            return encoded;
        }

        public string CaesarDecode(string encoded)
        {
            string decoded = String.Empty;
            foreach(char symbol in encoded)
            {
                decoded += (symbol == ' ') ? ' ' : CaesarCharEncode(symbol, 26 - Shift);
            }
            return decoded;
        }

        public string SimpleReplaceEncode()
        {
            string encoded = String.Empty;
            for(int i=0; i<Message.Length; ++i)
            {
                encoded += (Message[i] == ' ')?" ":(Alphabet[Message[i]].ToString() + " ");
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

        public void ReadFromFile(string fileName)
        {
            Message = "somethdin";
        }

        public static char cipher(char ch, int key)
        {
            if (!char.IsLetter(ch))
            {

                return ch;
            }

            char d = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((((ch + key) - d) % 26) + d);
        }

        public string Encode()
        {
            return "";
        }

        public string Decode()
        {
            return "";
        }
    }

    class Caesar
    {
        static void Main(string[] args)
        {
            Cipher cipher = new Cipher(5, Direction.Left, "Modest Buchick");
            Console.WriteLine(cipher.CaesarEncode());
            Console.WriteLine(cipher.CaesarDecode(cipher.CaesarEncode()));
            Console.ReadKey();
        }
    }
}
