using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;


namespace Age_of_Empires_ModLoader
{
    public class RasterImage
    {
        public int Width;
        public int Height;
        byte[] PixelData;

        //Constructors
        public RasterImage(int width, int height)
        {
            PixelData = new byte[width*height*4];
            Width = width;
            Height = height;
        }
        public RasterImage(byte[] pixelData, int width, int height)
        {
            PixelData = pixelData;
            Width = width;
            Height = height;
        }
        public RasterImage(string filePath)
        {
            FileInfo info = new FileInfo(filePath);
            if (info.Extension == ".tga")
            {
                Stream stream = new FileStream(filePath, FileMode.Open);//Open stream
                BinaryReader reader = new BinaryReader(stream);
                byte[] fileData = reader.ReadBytes((int)stream.Length);//Read all data from stream
                stream.Close();
                CreateFromTga(fileData);
            }
            else if (info.Extension == ".png" || info.Extension == ".jpg" || info.Extension == ".bmp")
            {
                CreateFromImage(filePath);
            }
        }
        public RasterImage(string imagePath,string alphaPath)
        {
            CreateFromImageAndAlphaMap(imagePath, alphaPath);
        }
        public RasterImage(RasterImage image, int width, int height)
        {
            Width = width;
            Height = height;
            PixelData = new byte[width * height * 4];
            float scaleWidth = (float)image.Width / width;
            float scaleHeight = (float)image.Height / height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = image.GetPixel((int)(x*scaleWidth), (int)(y*scaleHeight));
                    SetPixel(x, y, pixel);
                }
            }
        }

        //Set and Get pixel functions
        public void SetPixel(int x, int y, Color color)
        {
            PixelData[(x + y * Width) * 4] = color.R;
            PixelData[(x + y * Width) * 4 + 1] = color.G;
            PixelData[(x + y * Width) * 4 + 2] = color.B;
            PixelData[(x + y * Width) * 4 + 3] = color.A;
        }
        public Color GetPixel(int x, int y)
        {
            byte r = PixelData[(x + y * Width) * 4];
            byte g = PixelData[(x + y * Width) * 4 + 1];
            byte b = PixelData[(x + y * Width) * 4 + 2];
            byte a = PixelData[(x + y * Width) * 4 + 3];
            Color color = Color.FromArgb(a, r, g, b);
            return color;
        }

        //set pixeldata by reading tga file
        void CreateFromTga(byte[] fileData)
        {
            MemoryStream mStream = new MemoryStream(fileData);
            mStream.Skip(12);//Skip header (only uncompressed);
            Width = (int)BitConverter.ToUInt16(mStream.ReadBytes(2), 0);
            Height = (int)BitConverter.ToUInt16(mStream.ReadBytes(2), 0);
            byte bitsPerPixel = mStream.ReadByte();//Bits per pixel
            mStream.Skip(1);//discriptor

            PixelData = new byte[Height * Width * 4];

            //Image data
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    byte b = mStream.ReadByte();
                    byte g = mStream.ReadByte();
                    byte r = mStream.ReadByte();
                    byte a = mStream.ReadByte();
                    Color pixel = Color.FromArgb(a, r, g, b);
                    SetPixel(x, y, pixel);
                }
            }
        }

        //set pixeldata by reading image file
        void CreateFromImage(string filePath)
        {
            Bitmap bitmap = new Bitmap(filePath);
            Width = bitmap.Width;
            Height = bitmap.Height;
            PixelData = new byte[Height * Width * 4];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    SetPixel(x, y, pixel);
                }
            }
        }

        //set pixeldata by reading image file and alpha map file
        void CreateFromImageAndAlphaMap(string imagePath, string alphaPath)
        {
            Bitmap image = new Bitmap(imagePath);
            Bitmap alphaMap = new Bitmap(alphaPath);
            Width = image.Width;
            Height = image.Height;
            PixelData = new byte[Height * Width * 4];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    Color pixelAlpha = alphaMap.GetPixel(x, y);
                    Color pixel = Color.FromArgb(pixelAlpha.R, pixelColor.R, pixelColor.G, pixelColor.B);

                    SetPixel(x, y, pixel);
                }
            }
        }


        //Save functions

        //Save as image
        public void Save(string savePath,bool alpha)
        {
            Bitmap bitmap = new Bitmap(Width, Height);//create bitmap for save functions
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color pixel = GetPixel(x, y);
                    Color fullPixel = pixel;
                    if(!alpha)
                        fullPixel = Color.FromArgb(255, pixel.R, pixel.G, pixel.G);//set alpha to full
                    bitmap.SetPixel(x, y, fullPixel);
                }
            }
            bitmap.Save(savePath);
        }

        //Save alpha chanel as alpha map
        public void SaveAlphaMap(string savePath)
        {
            Bitmap bitmap = new Bitmap(Width, Height);//create bitmap for save functions
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color pixel = GetPixel(x, y);
                    Color fullPixel = Color.FromArgb(255, pixel.A, pixel.A, pixel.A);//Set all colors to alpha
                    bitmap.SetPixel(x, y, fullPixel);
                }
            }
            bitmap.Save(savePath);
        }

        //Save as tga file
        public void SaveTga(string savePath)
        {
            Stream stream = new FileStream(savePath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);

            //header
            writer.Write((byte)0);//Id lenght
            writer.Write((byte)0);//Color map type , 0  is no Color map
            writer.Write((byte)2);//2=uncompressed true color

            //Color map specification
            writer.Write((UInt16)0);//colormap offset ignore
            writer.Write((UInt16)0);//colormap lenght ignore
            writer.Write((byte)0);//colormap bits per pixel ignore

            //image specification
            writer.Write((UInt16)0);//origin x 
            writer.Write((UInt16)0);//origin y
            writer.Write((UInt16)Width);//image width
            writer.Write((UInt16)Height);//image height
            writer.Write((byte)32);//Bits per pixel
            writer.Write((byte)255);//descriptor 

            //Image data
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color pixel = GetPixel(x, y);
                    writer.Write(pixel.B);
                    writer.Write(pixel.G);
                    writer.Write(pixel.R);
                    writer.Write(pixel.A);
                }
            }

            stream.Close();
        }

        //Save as ddt file
        public void SaveDdt(string savePath)
        {
            //open stream and write file data
            Stream stream = new FileStream(savePath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Encoding.Default.GetBytes("RTS3"));//Magic word (constant)
            writer.Write((byte)0);//Usage unknown 0 or 1
            writer.Write((byte)8);//Alpha size 0=no aplha 8=8bit aplha
            writer.Write((byte)1);//compression 1 for uncompressed
            writer.Write((byte)1);//num mipmaps
            writer.Write(BitConverter.GetBytes(Width));//image width
            writer.Write(BitConverter.GetBytes(Height));//image height

            int dataOffset = 24;
            for (int i = 0; i < 1; i++)//foreach miplevel
            {

                int mipMapWidth = (int)(Width / Math.Pow((double)2, (double)i));
                int mipMapHeight = (int)(Height / Math.Pow((double)2, (double)i));
                if (mipMapWidth < 1)
                    mipMapWidth = 1;
                if (mipMapHeight < 1)
                    mipMapHeight = 1;
                int dataLenght = mipMapWidth * mipMapHeight * 4;
                writer.Write(BitConverter.GetBytes(dataOffset));//data offset per mipmap
                writer.Write(BitConverter.GetBytes(dataLenght));//data lenght per mipmap

                RasterImage mipMapRasterImage = new RasterImage(this, mipMapWidth, mipMapHeight);
                byte[] pixelData = new byte[dataLenght];
                for (int x = 0; x < mipMapRasterImage.Width; x++)
                {
                    for (int y = 0; y < mipMapRasterImage.Height; y++)
                    {
                        pixelData[(x + y * mipMapRasterImage.Width) * 4] = mipMapRasterImage.GetPixel(x, y).B;
                        pixelData[(x + y * mipMapRasterImage.Width) * 4 + 1] = mipMapRasterImage.GetPixel(x, y).G;
                        pixelData[(x + y * mipMapRasterImage.Width) * 4 + 2] = mipMapRasterImage.GetPixel(x, y).R;
                        pixelData[(x + y * mipMapRasterImage.Width) * 4 + 3] = mipMapRasterImage.GetPixel(x, y).A;
                    }
                }
                writer.Write(pixelData);//pixeldata
                dataOffset += dataLenght;
            }
            stream.Close();
        }

        //Convert RasterImage to Bitmap type
        public Bitmap ToBitmap(int width,int height)
        {
            Bitmap originalBitmap = new Bitmap(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    originalBitmap.SetPixel(x, y, GetPixel(x, y));
                }
            }
            Bitmap resizedBitmap = new Bitmap(originalBitmap, width, height);
            return resizedBitmap;
        }
    }
}
