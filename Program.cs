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

            Console.WriteLine(Encoding.UTF8.GetString(magic));
            if (Encoding.UTF8.GetString(magic, 0, 4) == "Vxvs")
            {
                file.Read(magic, 0, 4);
                int ten = BitConverter.ToInt32(magic, 0);

                file.Read(magic, 0, 8);

                if (Encoding.UTF8.GetString(magic) != "0.0.3")
                {
                    Console.WriteLine("Unteseted version!");
                }

                byte[] stuff = new byte[8];
                file.Read(stuff, 0, 4);

                int dataStart = BitConverter.ToInt32(stuff, 0);

                file.Read(stuff, 0, 4);

                int dataEnd = BitConverter.ToInt32(stuff, 0);

                int dataLength = dataEnd - dataStart;

                byte[] dataBuffer = new byte[dataLength];

                if (crack)
                {
                    // Skip to the data and scan it for data.

                    int buffer;

                    int bufferPosition = 0;

                    int magicFound = 0;

                    int fileFound = 0;

                    int headersFound = 0;

                    file.Seek(dataStart, SeekOrigin.Begin);

                    FileStream output;

                    while ((buffer = file.ReadByte()) != -1)
                    {
                        byte nextByte = (byte)buffer;

                        dataBuffer[bufferPosition++] = (byte)buffer;

                        if (buffer == 'R' || (magicFound == 1 && buffer == 'I') || ((magicFound == 2 || magicFound == 3) && buffer == 'F')) magicFound++;
                        else magicFound = 0;

                        if (magicFound == 4)
                        {
                            headersFound++;
                            magicFound = 0;

                            if (headersFound > 1)
                            {
                                headersFound = 0;
                                bufferPosition -= 4;
                                file.Seek(-4, SeekOrigin.Current);
                                output = File.OpenWrite("filefound" + fileFound++ + ".wav");

                                output.Write(dataBuffer, 0, bufferPosition);
                            }
                        }

                    }

                    output = File.OpenWrite("filefound" + fileFound++ + ".wav");
                    output.Write(dataBuffer, 0, bufferPosition);
                }
                else
                {
                    // Read the file as intended.
                    byte[] data = new byte[4];
                    file.Read(data, 0, 4);

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

                    
                }
                else
                {
                    Console.WriteLine("Invalid file.");
                }
            Console.ReadLine();
        }
    }
}
