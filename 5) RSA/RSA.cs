using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5__RSA
{
    class RSATask
    {
        private static Dictionary<char, int> Alphabet { get; set; }
        private static void GenerateAlphabet()
        {
            Alphabet = new Dictionary<char, int>();
            char[] alphabet = new char[33] { 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Є', 'Ж', 'З', 'И', 'І', 'Ї', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ю', 'Я', ' ' };
            for (int i = 0; i < alphabet.Length; ++i)
            {
                Alphabet.Add(alphabet[i], i);
            }
        }

        private static string Encode(string message, int E, int n)
        {
            string result = String.Empty;
            foreach(char symbol in message)
            {
                int num = Alphabet[symbol];
                result += Math.Pow(num, E) % n + " ";
            }
            result = result.Substring(0, result.Length - 1);
            return result;
        }

        private static string Decode(string message, int D, int n)
        {
            string result = String.Empty;
            string[] digits = message.Split(' ');
            for(int i=0; i<digits.Length; ++i)
            {
                int d = Convert.ToInt32(digits[i]);
                int r = (int)(Math.Pow(d, D) % n);
                result += Alphabet.FirstOrDefault(x => x.Value == r).Key;
            }
            return result;
        }

        private static int HashDigest(string message, int n)
        {
            int sum = 0;
            foreach(char symbol in message)
            {
                sum += Alphabet[symbol];
            }
            return sum % n;
        }

        private static int DigitalSignature(int hash, int E, int n)
        {
            return (int)(Math.Pow(hash, E) % n);
        }


        private static void Main(string[] args)
        {
            Console.WriteLine("Encoding:::::::::::::::::::::::::");
            GenerateAlphabet();
            int p = 3;
            int q = 11;
            int n = p * q;
            int E = 7;
            int D = 3;
            string message = File.ReadAllLines("../../InputMessage.txt")[0];
            string encoded = Encode(message, E, n);
            Console.WriteLine("Encoded: " + encoded);
            int hash = HashDigest(message, n);
            Console.WriteLine("Hash: " + hash);
            int digitalSignature = DigitalSignature(hash, E, n);
            Console.WriteLine("DigitalSignature: " + digitalSignature);
            using (StreamWriter writer = new StreamWriter("../../Encoded.txt"))
            {
                writer.WriteLine(encoded);
                writer.WriteLine(digitalSignature);
            }
            Console.ReadKey();
            Console.WriteLine("\nDecoding:::::::::::::::::::::::::");
            string encodedMessage;
            int digitalSignatureFromFile = 0;
            using (StreamReader reader = new StreamReader("../../Encoded.txt"))
            {
                encodedMessage = reader.ReadLine();
                digitalSignatureFromFile = Convert.ToInt32(reader.ReadLine());
            }
            Console.WriteLine("Encoded message: " + encodedMessage);
            Console.WriteLine("Digital signature: " + digitalSignature);
            string decoded = Decode(encodedMessage, D, n);
            Console.WriteLine("Decoded: " + decoded);
            int decodedHash = HashDigest(decoded, n);
            Console.WriteLine("Decoded hash: " + decodedHash);
            int hashFromDigitalSignature = DigitalSignature(digitalSignature, D,n);
            Console.WriteLine("Hash from digital signature: " + hashFromDigitalSignature);
            Console.WriteLine("Equals: " + ((hashFromDigitalSignature == decodedHash) ? "yes" : "no"));
            Console.ReadKey();
        }
    }
}
