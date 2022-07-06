using System.Collections.Generic;
using System.IO;
using ArgonVisual.Helpers;

namespace ArgonVisual.DocumentItems;

/// <summary>
/// Represents a class that contains functions and properties
/// </summary>
public class ArgonClass
{
    /// <summary>
    /// The name of the class
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The functions in this class
    /// </summary>
    public List<ArgonFunction> Functions { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ArgonClass"/> with the name.
    /// </summary>
    /// <param name="name">The name of the class.</param>
    public ArgonClass(string name) 
    {
        Name = name;
        Functions = new List<ArgonFunction>();
    }

    public static ArgonClass Read(BinaryReader reader, ArgonCodeFile.Version version) 
    {
        string typename = reader.ReadString();
        ArgonClass argonClass = new ArgonClass(typename);

        if (version >= ArgonCodeFile.Version.SerializeFunctions)
        {
            reader.ReadArray(argonClass.Functions, () => ArgonFunction.Read(reader));
        }

        return argonClass;
    }

    public void Write(BinaryWriter writer) 
    {
        writer.Write(Name);
        writer.WriteArray(Functions, (item) => item.Write(writer));
    }
}
