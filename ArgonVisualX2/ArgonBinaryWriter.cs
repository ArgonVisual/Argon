using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ArgonVisualX2;

public class ArgonBinaryWriter : BinaryWriter
{
    public ArgonBinaryWriter(Stream output) : base(output)
    {
        Write((uint)SerializationVersion.Latest);
    }

    public void WriteArray<T>(IEnumerable<T> items, Action<T> writeItem) 
    {
        Write((int)items.Count());

        foreach (T item in items)
        {
            writeItem(item);
        }
    }
}