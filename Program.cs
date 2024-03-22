//Written for 9 Dragons. https://store.steampowered.com/app/390100/
using System;
using System.IO;

namespace _9_Dragons_Extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            BinaryReader br = new(File.OpenRead(args[0]));
            int magic = br.ReadInt32();
            if (magic != -1610481926)
                throw new Exception("Not a 9 Dragons XP file.");

            int fileCount = br.ReadInt32();
            System.Collections.Generic.List<byte[]> data = new();
            for (int i = 0; i < fileCount; i++)
                data.Add(br.ReadBytes(br.ReadInt16()));

            System.Collections.Generic.List<int> offsets = new();
            for (int i = 0; i < fileCount; i++)
                offsets.Add(br.ReadInt32());

            FileStream FS;
            BinaryWriter bw;
            Directory.CreateDirectory(Path.GetDirectoryName(args[0]) + "//" + Path.GetFileNameWithoutExtension(args[0]));

            for (int i = 0; i < fileCount - 1; i++)
            {
                FS = File.Create(Path.GetDirectoryName(args[0]) + "//" + Path.GetFileNameWithoutExtension(args[0]) + "//" + i);
                bw = new(FS);
                bw.Write(br.ReadBytes(offsets[i +1] - offsets[i]));
                bw.Close();
                FS.Close();
            }
            FS = File.Create(Path.GetDirectoryName(args[0]) + "//" + Path.GetFileNameWithoutExtension(args[0]) + "//" + (fileCount - 1));
            bw = new(FS);
            bw.Write(br.ReadBytes((int)(br.BaseStream.Length - offsets[^1])));
            bw.Close();
            FS.Close();
        }
    }
}
