using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2__Gamma_cipher
{
    public class GammaCipher
    {
        private Dictionary<char, int> Alphabet { get; set; }
        public string InputMessage { get; set; }
        private int[] MessageSimpleReplace { get; set; }
        private int[] PseudoGamma { get; set; }
        private int[] Key { get; set; }

        public GammaCipher()
        {
            GenerateAlphabet();
            Key = new int[3];
        }

        private void GenerateAlphabet()
        {
            Alphabet = new Dictionary<char, int>();
            Random indexRandomizer = new Random();
            int number = 0;
            for (int i = 65; i <= 122; ++i)
            {
                if (!(i > 90 && i < 97))
                {
                    Alphabet.Add((char)i, number++);
                }
            }
        }

        public string ReadMessageFromFile(string fileName)
        {
            using (StreamReader reader = new StreamReader("../../" + fileName))
            {
                InputMessage = reader.ReadLine();
            }
            return InputMessage;
        }

        public void WriteEncodedToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter("../../" + fileName))
            {
                writer.WriteLine(Encode());
            }
        }

        public void ReadEncodedFromFile(string fileName)
        {
            using (StreamReader reader = new StreamReader("../../" + fileName))
            {
                InputMessage = reader.ReadLine();
            }
        }

        public void WriteDecodedToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter("../../" + fileName))
            {
                writer.WriteLine(Decode());
            }
        }

        public string SimpleReplace()
        {
            MessageSimpleReplace = new int[InputMessage.Length];
            for(int i=0; i<InputMessage.Length; ++i)
            {
                if(InputMessage[i]==' ')
                {
                    MessageSimpleReplace[i] = Alphabet['Z'];
                }
                else
                {
                    MessageSimpleReplace[i] = Alphabet[InputMessage[i]];
                }
            }
            string replaceResult = String.Empty;
            for(int i=0; i<MessageSimpleReplace.Length; ++i)
            {
                replaceResult += MessageSimpleReplace[i] + " ";
            }
            return replaceResult;
        }

        public void ReadKey(string key)
        {
            string[] keyElems = key.Split(' ');
            for(int i=0; i<3; ++i)
            {
                Key[i] = Convert.ToInt32(keyElems[i]);
            }
        }

        public string GenerateGamma(int size)
        {
            int[] Gamma = new int[size + 1];
            PseudoGamma = new int[size];
            //Generating Gamma
            for(int i=0; i<3; ++i)
            {
                Gamma[i] = Key[i];
            }
            for(int i=3; i< size + 1; ++i)
            {
                Gamma[i] = (Gamma[i-1]+Gamma[i-3])%52;
            }

            //Generation PseudoGamma
            for (int i=0; i< size; ++i)
            {
                PseudoGamma[i] = (Gamma[i] + Gamma[i + 1]) % 52;
            }

            string gammaResult = String.Empty;
            for(int i=0; i< PseudoGamma.Length; ++i)
            {
                gammaResult += PseudoGamma[i] + " ";
            }
            return gammaResult;
        }

        public string Encode()
        {
            string encodedMessage = String.Empty;
            for(int i=0;i<MessageSimpleReplace.Length; ++i)
            {
                encodedMessage += (MessageSimpleReplace[i] + PseudoGamma[i]) % 52 + " ";
            }
            encodedMessage = encodedMessage.Substring(0, encodedMessage.Length - 1);
            return encodedMessage;
        }

        public string Decode()
        {
            string decodedMessage = String.Empty;
            string[] messageArrayStr = InputMessage.Split(' ');
            int[] messageArray = new int[messageArrayStr.Length];
            MessageSimpleReplace = new int[messageArray.Length];
            for(int i=0; i<messageArrayStr.Length; ++i)
            {
                messageArray[i] = Convert.ToInt32(messageArrayStr[i]);
            }
            for(int i=0; i<messageArray.Length; ++i)
            {
                MessageSimpleReplace[i] = (messageArray[i] + (52 - PseudoGamma[i])) % 52;
                decodedMessage += Alphabet.FirstOrDefault(x => x.Value == MessageSimpleReplace[i]).Key;
            }
            decodedMessage = decodedMessage.Replace("Z", " ");
            return decodedMessage;
        }
    }

    class Gamma
    {
        static void Main(string[] args)
        {
            GammaCipher cipher = new GammaCipher();
            Console.WriteLine("ENCODING#####################");
            Console.WriteLine("Input message: "+cipher.ReadMessageFromFile("InputMessage.txt"));
            Console.WriteLine("Siple replace: "+cipher.SimpleReplace());
            Console.WriteLine("Input key (0-51) (exapmle 23 12 42): ");
            cipher.ReadKey(Console.ReadLine());
            Console.WriteLine("Gamma: " + cipher.GenerateGamma(cipher.InputMessage.Length));
            Console.WriteLine("Encoded: " + cipher.Encode());
            cipher.WriteEncodedToFile("Encoded.txt");
            Console.WriteLine("Encoded message writen to file Encoded.txt!!!");
            Console.ReadKey();
            Console.WriteLine("\nDECODING####################");
            cipher.ReadEncodedFromFile("Encoded.txt");
            Console.WriteLine("Encoded from file: " + cipher.InputMessage);
            Console.WriteLine("Input key (0-51) (exapmle 23 12 42): ");
            cipher.ReadKey(Console.ReadLine());
            Console.WriteLine("Gamma: " + cipher.GenerateGamma(cipher.InputMessage.Split(' ').Length));
            Console.WriteLine("Decoded: " + cipher.Decode());
            cipher.WriteDecodedToFile("Decoded.txt");
            Console.WriteLine("Decoded message writen to file Decoded.txt!!!");
            Console.ReadKey();
        }
    }
}
