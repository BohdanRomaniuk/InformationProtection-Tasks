﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3__BBS
{
    class BBSProgram
    {
        public static byte[] ConvertStringToBytes(string message)
        {
            return Encoding.Default.GetBytes(message);
        }

        public static string ConvertBytesToString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        public static byte ConvertToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }

        public static byte[] BBS(int messageLength, int n, int x)
        {
            BitArray bits = new BitArray(messageLength * 8);
            byte[] result = new byte[messageLength];
            int x0 = (x*x) % n;
            bits[0] = ((x0 * x0) % 2 == 1);
            for(int i=1; i<bits.Length; ++i)
            {
                bits[i] = ((x0 * x0) % 2 == 1);
                x0 = (x0 * x0) % n;
            }
            bits.CopyTo(result, 0);
            return result;
        }

        public static byte[] Xor(byte[] message, byte[] bbs)
        {
            byte[] resultBuffer = new byte[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                resultBuffer[i] = (byte)(message[i] ^ bbs[i]);
            }
            return resultBuffer;
        }

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Input p=");
                int p = Convert.ToInt32(Console.ReadLine());
                Console.Write("Input q=");
                int q = Convert.ToInt32(Console.ReadLine());
                if (p % 4 != 3 || q % 4 != 3)
                {
                    throw new ArgumentException("P and Q must be =3mod4!!!");
                }
                int n = p * q;
                Console.Write("Input x=");
                int x = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Input message:");
                string message = Console.ReadLine();
                byte[] bytes = ConvertStringToBytes(message);
                Console.Write("Bits: ");
                for(int i=0; i<bytes.Length; ++i)
                {
                    Console.Write(Convert.ToString(bytes[i], 2).PadLeft(8, '0')+" ");
                }
                Console.Write("\nBBS:  ");
                byte[] bbs = BBS(message.Length, n, x);
                for(int i=0; i<bbs.Length; ++i)
                {
                    Console.Write(Convert.ToString(bbs[i], 2).PadLeft(8, '0') + " ");
                }
                byte[] encoded = Xor(bytes, bbs);
                Console.Write("\nXor:  ");
                for (int i = 0; i < bbs.Length; ++i)
                {
                    Console.Write(Convert.ToString(encoded[i], 2).PadLeft(8, '0') + " ");
                }
                string encodedMessage = ConvertBytesToString(encoded);
                Console.WriteLine("\nEncoded: " + encodedMessage);

                byte[] decoded = Xor(encoded, bbs);
                string decodedMessage = ConvertBytesToString(decoded);
                Console.WriteLine("Decode: " + decodedMessage);
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR!!! "+ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
