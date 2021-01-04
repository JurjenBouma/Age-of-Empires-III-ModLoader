using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Age_of_Empires_ModLoader
{
    public class MemoryStream
    {
        int position;
        byte[] bytes;
        public MemoryStream(byte[] data)
        {
            bytes = data;
            position = 0;
        }

        public int GetPosition() { return position; }

        public byte ReadByte()
        {
            byte returnByte;
            if (position < bytes.Length)
            {
                returnByte = bytes[position];
                position++;
            }
            else
            {
                returnByte = 0;
            }
            return returnByte;
        }
        public byte[] ReadBytes(int lenght)
        {
            byte[] returnBytes = new byte[lenght];
            for (int i = 0; i < lenght; i++)
            {
                if (position < bytes.Length)
                {
                    returnBytes[i] = bytes[position];
                    position++;
                }
                else
                {
                    returnBytes[i] = 0;
                }
            }
            return returnBytes;
        }

        public void Skip(int Positions)
        {
            position += Positions;
        }

        public void SetPosition(int Position)
        {
            position = Position;
        }
    }

    static class ByteAnalyzer
    {
        public static byte[] GetBitsFromNumber(int inputInt)
        {
            byte[] bits = new byte[32];
            for (int i = 31; i >= 0; i--)
            {
                bits[i] = (byte)(inputInt / Math.Pow((double)2, (double)i));
                inputInt -= (int)(Math.Pow((double)2, (double)i) * bits[i]);
            }

            return bits;
        }

        public static byte[] GetBitsFromNumber(byte inputByte)
        {
            byte[] bits = new byte[8];
            for (int i = 7; i >= 0; i--)
            {
                bits[i] = (byte)(inputByte / Math.Pow((double)2, (double)i));
                inputByte -= (byte)(Math.Pow((double)2, (double)i) * bits[i]);
            }

            return bits;
        }

        public static int GetIntFromBits(byte[] bits)
        {
            int returnInt = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                returnInt += (int)(bits[i] * Math.Pow((double)2, (double)i));
            }
            return returnInt;
        }
        public static byte GetByteFromBits(byte[] bits)
        {
            byte returnInt = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                returnInt += (byte)(bits[i] * Math.Pow((double)2, (double)i));
            }
            return returnInt;
        }
    }
}
