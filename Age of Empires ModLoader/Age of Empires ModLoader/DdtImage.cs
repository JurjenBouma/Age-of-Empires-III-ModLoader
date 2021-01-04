using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Age_of_Empires_ModLoader
{
    class DdtImage
    {
        byte[] FileData;
        public DdtImage(byte[] data)
        {
            FileData = data;
        }

        public string ReadHeader()
        {
            //open stream and read file data
            MemoryStream mStream = new MemoryStream(FileData);
            mStream.ReadBytes(5);//skip
            byte alpha = mStream.ReadByte();
            byte compression = mStream.ReadByte();
            byte mipLevels = mStream.ReadByte();
            int width = BitConverter.ToInt32(mStream.ReadBytes(4),0);
            int height = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
            return "Alpha=" + alpha.ToString() + "\nCompression=" + compression.ToString() + "\nMipLevels=" + mipLevels.ToString() + "\nWidth=" + width.ToString() + "\nHeight=" + height.ToString();
        }

        public int GetMipLevels()
        {
            //open stream and read file data
            MemoryStream mStream = new MemoryStream(FileData);
            mStream.ReadBytes(7);//skip
            byte mipLevels = mStream.ReadByte();
            return (int)mipLevels;
        }

        public RasterImage ToRasterImage()
        {
            //open stream and read file data
            MemoryStream mStream = new MemoryStream(FileData);
            mStream.ReadBytes(6);//skip
            byte compression = mStream.ReadByte();//Compression Type
            if (compression == 1)//uncompressed
                return GetUncompressedRasterImage();
            else if (compression == 4)//DXT1
                return GetDXT1RasterImage();
            else if (compression == 8)//DXT3
                return GetDXT3RasterImage();
            else
                return null;
        }

        RasterImage GetUncompressedRasterImage()
        {
            //open stream and read file data
            MemoryStream mStream = new MemoryStream(FileData);
            mStream.ReadBytes(6);//skip
            byte compression = mStream.ReadByte();//Compression Type
            if (compression == 1)//uncompressed
            {
                int mipLevels = mStream.ReadByte();//number of Miplevels
                int imageWidth = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int imageHeight = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int offset = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int dataLenght = BitConverter.ToInt32(mStream.ReadBytes(4), 0);

                //Get pixeldata
                mStream.SetPosition(offset);
                RasterImage image = new RasterImage(imageWidth, imageHeight);
                for (int y = 0; y < imageHeight; y++)
                {
                    for (int x = 0; x < imageWidth; x++)
                    {
                        int B = mStream.ReadByte();
                        int G = mStream.ReadByte();
                        int R = mStream.ReadByte();
                        int A = mStream.ReadByte();
                        image.SetPixel(x, y, Color.FromArgb(A, R, G, B));
                    }
                }
                return image;
            }
            else
                return null;
        }

        RasterImage GetDXT1RasterImage()
        {
            //open stream and read file data
            MemoryStream mStream = new MemoryStream(FileData);
            mStream.ReadBytes(6);//skip
            byte compression = mStream.ReadByte();//Compression Type

            if (compression == 4)//DXT1
            {
                int mipLevels = mStream.ReadByte();//number of Miplevels
                //get header values
                int imageWidth = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int imageHeight = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int offset = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int dataLenght = BitConverter.ToInt32(mStream.ReadBytes(4), 0);

                //Get pixeldata
                mStream.SetPosition(offset);
                RasterImage image = new RasterImage(imageWidth, imageHeight);
                int xOffset = 0;
                int yOffset = 0;
                for (int i = 0; i < dataLenght; i += 8)
                {
                    //Get 4 colors
                    UInt16 iC0 = BitConverter.ToUInt16(mStream.ReadBytes(2), 0);//16bit Color c0 5:6:5 RGB
                    UInt16 iC1 = BitConverter.ToUInt16(mStream.ReadBytes(2), 0);//16bit Color c1 5:6:5 RGB
                    Color[] colors = GetColorsFromDXTFormat(iC0, iC1, true);//Calculate the 4 colors

                    //Get 4x4 Pixelblock and set Pixels in bitmap
                    for (int y = 0; y < 4; y++)
                    {
                        List<byte> pointerBits = new List<byte>(ByteAnalyzer.GetBitsFromNumber((int)mStream.ReadByte()));//byte holding 4 2bit Color pointers
                        for (int x = 0; x < 4; x++)
                        {
                            int colorPointer = ByteAnalyzer.GetIntFromBits(pointerBits.GetRange(x * 2, 2).ToArray());//Pointer for pixel
                            image.SetPixel(xOffset + x, yOffset + y, colors[colorPointer]);
                        }
                    }

                    // set xOffset and yOffset values
                    if (xOffset + 4 < imageWidth)
                        xOffset += 4;
                    else
                    {
                        xOffset = 0;
                        yOffset += 4;
                    }

                }
                return image;
            }
            else
                return null;

        }

        RasterImage GetDXT3RasterImage()
        {
            //open stream and read file data
            MemoryStream mStream = new MemoryStream(FileData);
            mStream.ReadBytes(6);//skip
            byte compression = mStream.ReadByte();//Compression Type

            if (compression == 8)//DXT3
            {
                int mipLevels = mStream.ReadByte();//number of Miplevels
                //get header values
                int imageWidth = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int imageHeight = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int offset = BitConverter.ToInt32(mStream.ReadBytes(4), 0);
                int dataLenght = BitConverter.ToInt32(mStream.ReadBytes(4), 0);

                //Get pixeldata
                mStream.SetPosition(offset);
                RasterImage image = new RasterImage(imageWidth, imageHeight);
                int xOffset = 0;
                int yOffset = 0;
                for (int i = 0; i < dataLenght; i += 16)
                {
                    //Get Alpha values
                    List<byte> alphaBytes = new List<byte>(mStream.ReadBytes(8));

                    //Get 4 colors
                    UInt16 iC0 = BitConverter.ToUInt16(mStream.ReadBytes(2), 0);//16bit Color c0 5:6:5 RGB
                    UInt16 iC1 = BitConverter.ToUInt16(mStream.ReadBytes(2), 0);//16bit Color c1 5:6:5 RGB
                    Color[] colors = GetColorsFromDXTFormat(iC0, iC1, false);//Calculate the 4 colors

                    //Get 4x4 Pixelblock and set Pixels in bitmap
                    for (int y = 0; y < 4; y++)
                    {
                        List<byte> pointerBits = new List<byte>(ByteAnalyzer.GetBitsFromNumber((int)mStream.ReadByte()));//Convert byte holding 4 2bit Color pointers to bit list
                        List<byte> alphaBits = new List<byte>(ByteAnalyzer.GetBitsFromNumber((alphaBytes[y * 2])));//Convert bytes holding 2 4bit Alpha value each, to bit list
                        alphaBits.AddRange(ByteAnalyzer.GetBitsFromNumber(alphaBytes[y * 2 + 1]));//Add secon Byte-bits

                        for (int x = 0; x < 4; x++)
                        {
                            int colorPointer = ByteAnalyzer.GetIntFromBits(pointerBits.GetRange(x * 2, 2).ToArray());//Pointer for pixel
                            byte alpha = (byte)((float)ByteAnalyzer.GetIntFromBits(alphaBits.GetRange(x * 4, 4).ToArray()) / 15 * 255);//Calulate alpha value byte
                            Color color = Color.FromArgb(alpha, colors[colorPointer].R, colors[colorPointer].G, colors[colorPointer].B);

                            image.SetPixel(xOffset + x, yOffset + y, color);
                        }
                    }

                    // set xOffset and yOffset values
                    if (xOffset + 4 < imageWidth)
                        xOffset += 4;
                    else
                    {
                        xOffset = 0;
                        yOffset += 4;
                    }

                }
                return image;
            }
            else
                return null;

        }

        Color[] GetColorsFromDXTFormat(UInt16 iC0, UInt16 iC1, bool DXT1)
        {
            //Get 4 colors
            List<byte> bitsC0 = new List<byte>(ByteAnalyzer.GetBitsFromNumber((int)iC0));//List of bit values as bytes 0 and 1;
            List<byte> bitsC1 = new List<byte>(ByteAnalyzer.GetBitsFromNumber((int)iC1));//List of bit values as bytes 0 and 1;

            Color[] colors = new Color[4];

            byte[] bits = bitsC0.GetRange(0, 5).ToArray();//Blue bits
            byte b0 = (byte)((float)ByteAnalyzer.GetIntFromBits(bits) / 31 * 255);//Convert to byte
            bits = bitsC0.GetRange(5, 6).ToArray();//Green bits
            byte g0 = (byte)((float)ByteAnalyzer.GetIntFromBits(bits) / 63 * 255);//Convert to byte
            bits = bitsC0.GetRange(11, 5).ToArray();//Red bits
            byte r0 = (byte)((float)ByteAnalyzer.GetIntFromBits(bits) / 31 * 255);//Convert to byte
            colors[0] = Color.FromArgb(r0, g0, b0);

            bits = bitsC1.GetRange(0, 5).ToArray();//Blue bits
            byte b1 = (byte)((float)ByteAnalyzer.GetIntFromBits(bits) / 31 * 255);//Convert to byte
            bits = bitsC1.GetRange(5, 6).ToArray();//Green bits
            byte g1 = (byte)((float)ByteAnalyzer.GetIntFromBits(bits) / 63 * 255);//Convert to byte
            bits = bitsC1.GetRange(11, 5).ToArray();//Red bits
            byte r1 = (byte)((float)ByteAnalyzer.GetIntFromBits(bits) / 31 * 255);//Convert to byte
            colors[1] = Color.FromArgb(r1, g1, b1);

            //Calculate Colors c2 and c3
            if (iC0 > iC1 || !DXT1)
            {
                byte b2 = (byte)((float)b0 / 3 * 2 + (float)b1 / 3);
                byte g2 = (byte)((float)g0 / 3 * 2 + (float)g1 / 3);
                byte r2 = (byte)((float)r0 / 3 * 2 + (float)r1 / 3);
                colors[2] = Color.FromArgb(r2, g2, b2);

                byte b3 = (byte)((float)b0 / 3 + (float)b1 / 3 * 2);
                byte g3 = (byte)((float)g0 / 3 + (float)g1 / 3 * 2);
                byte r3 = (byte)((float)r0 / 3 + (float)r1 / 3 * 2);
                colors[3] = Color.FromArgb(r3, g3, b3);
            }
            else
            {
                byte b2 = (byte)((float)b0 / 2 + (float)b1 / 2);
                byte g2 = (byte)((float)g0 / 2 + (float)g1 / 2);
                byte r2 = (byte)((float)r0 / 2 + (float)r1 / 2);
                colors[2] = Color.FromArgb(r2, g2, b2);
                colors[3] = Color.FromArgb(0, 0, 0, 0);
            }
            return colors;
        }
    }
}
