
using System;
using System.Collections.Generic;
using System.IO;

namespace ArgonVisual.Helpers;

public static class SerializationHelper 
{
    public static void ReadArray<T>(this BinaryReader reader, List<T> list, Func<T> createItem) 
    {
        uint arraySize = reader.ReadUInt32();
        for (int i = 0; i < arraySize; i++)
        {
            list.Add(createItem());
        }
    }

    public static void WriteArray<T>(this BinaryWriter writer, List<T> list, Action<T> writeItem)
    {
        writer.Write((uint)list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            writeItem(list[i]);
        }
    }
}