using System;
using System.Collections.Generic;
using System.IO;

namespace ArgonVisualX2;

public class ArgonBinaryReader : BinaryReader
{
    public SerializationVersion Version { get; }

    public ArgonBinaryReader(Stream input) : base(input)
    {
        Version = (SerializationVersion)ReadUInt32();
    }

    public void ReadArray<T>(List<T> items, Func<T> readItem) 
    {
        int count = ReadInt32();
        for (int i = 0; i < count; i++)
        {
            items.Add(readItem());
        }
    }
}