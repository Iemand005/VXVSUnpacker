using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VXVSUnpacker
{
    internal class Program
    {
        static void Main(string[] args)
        {

            bool crack = true;

            FileStream file = File.OpenRead("etron.vxvs");

            byte[] magic = new byte[8];
            file.Read(magic, 0, 4);

            if (Encoding.UTF8.GetString(magic) == "Vxvs")
            {
                file.Read(magic, 0, 4);

                StringBuilder stringBuilder = new StringBuilder();
                int versionChunk;

                while ((versionChunk = file.ReadByte()) != 0) stringBuilder.Append(versionChunk);

                if (stringBuilder.ToString() != "0.0.3")
                {
                    Console.WriteLine("Unteseted version!");
                }

                byte[] stuff = new byte[8];
                file.Read(stuff, 0, 8);

                byte[] thingLength = new byte[2];
                file.Read(thingLength, 0, 2);

                short length = BitConverter.ToInt16(thingLength, 0);

                byte[] data = new byte[length];
                file.Read(data, 0, length);

                Console.WriteLine(Encoding.UTF8.GetString(data));

                file.Read(magic, 0, 4);
                file.Read(magic, 0, 4);

                int segmentCount = BitConverter.ToInt32(magic, 0);

                Console.WriteLine(segmentCount + "segments");

                byte[] another = new byte[28];

                for (int i = 0; i < segmentCount; ++i)
                {
                    file.Read(another, 0, 28);
                }

                file.Read(magic, 0, 4);
                Console.WriteLine(Encoding.UTF8.GetString(magic));

                if (Encoding.UTF8.GetString(magic) == "Cfgs")
                {
                    //byte[] thing = new byte[48];
                    //file.Read(thing, 0, 48);

                    file.Read(stuff, 0, 4);
                    file.Read(stuff, 0, 8);

                    byte[] nameBuffer = new byte[32];
                    file.Read(nameBuffer, 0, 32);
                    Console.WriteLine("Found thing: " + nameBuffer);

                    file.Read(magic, 0, 4);
                    file.Read(magic, 0, 4);

                    string conf = Encoding.UTF8.GetString(magic);
                    Console.WriteLine(conf);
                    if (conf == "Conf")
                    {

                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid file.");
            }
            Console.ReadLine();
        }
    }
}
