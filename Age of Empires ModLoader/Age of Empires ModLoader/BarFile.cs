using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace Age_of_Empires_ModLoader
{
    public struct AoEFile
    {
        public int fileOffset;
        public int fileSize;
        public string fileName;
    }
    public class BarFile
    {
        public string DirectoryName;
        string FilePath;
        public List<AoEFile> Files;

        //Returns a list of folders containing fileNames and their file info
        public BarFile(string filePath)
        {
            FilePath = filePath;

            Stream stream = new FileStream(filePath, FileMode.Open);
            BinaryReader reader = new BinaryReader(stream);
            reader.ReadBytes(280);//Magic word,version,empy....
            int fileCountTotal = BitConverter.ToInt32(reader.ReadBytes(4), 0);//number of files
            int directoryOffset = BitConverter.ToInt32(reader.ReadBytes(4), 0);//offset to main directory
            stream.Position = directoryOffset;//go to director offset

            //Fetch directory name
            int mainDirectoryNameLenght = BitConverter.ToInt32(reader.ReadBytes(4), 0);//lenght if the directory name
            DirectoryName = "";
            for (int i = 0; i < mainDirectoryNameLenght * 2; i++)
            {
                Char c = (Char)reader.ReadByte();
                if (c != '\0')
                    DirectoryName += c;
            }
            //Add folder to list
            Files = new List<AoEFile>();

            //Add files to folder
            int fileCountDirectory = BitConverter.ToInt32(reader.ReadBytes(4), 0);//fileCount directory
            for (int i = 0; i < fileCountDirectory; i++)
            {
                AoEFile file = new AoEFile();
                file.fileOffset = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                file.fileSize = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                reader.ReadBytes(20);//skip
                file.fileName = "";
                int fileNameLenght = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                for (int chr = 0; chr < fileNameLenght * 2; chr++)
                {
                    Char c = (Char)reader.ReadByte();
                    if (c != '\0')
                        file.fileName += c;
                }
                //Add file to foler's fileList
                Files.Add(file);
            }
            stream.Close();
        }

        //Get the file from Bar file and save it to savePath
        public byte[] ReadFile(AoEFile file)
        {
            //Setup stream
            Stream readStream = new FileStream(FilePath, FileMode.Open);
            BinaryReader reader = new BinaryReader(readStream);

            //Get file data
            readStream.Position = file.fileOffset;
            byte[] fileBytes = reader.ReadBytes(file.fileSize);
            readStream.Close();

            return fileBytes;
        }
    }
}
