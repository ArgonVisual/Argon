using System.Collections.Generic;
using System.IO;
using Argon.Helpers;

namespace Argon.FileTypes;

/// <summary>
/// Represents a file in memory that contains code.
/// </summary>
public class ArgonCodeFile : IFileHandle
{
    /// <summary>
    /// The serialization version for the binary format
    /// </summary>
    public enum Version : uint
    {
        FirstCodeFile,
        SerializeContainers,

        Last,
        Latest = Last - 1,
    }

    /// <summary>
    /// The full name of the project on disk.
    /// </summary>
    public string Filename => Path.Combine(Directory, Name) + FileExtensions.CodeFile;

    /// <summary>
    /// The name of the project
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The directory on disk that the project is contained inside of.
    /// </summary>
    public string Directory { get; }

    /// <summary>
    /// The classes and structs that are in this file.
    /// </summary>
    public List<ArgonClass> Containers { get; }

    public ArgonCodeFile(string filename)
    {
        (Directory, Name) = filename.GetDirectoryAndName();
        Containers = new List<ArgonClass>();
    }

    public void Save() 
    {
        using FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write);
        using BinaryWriter writer = new BinaryWriter(fileStream);

        // Write the serialization version of the file
        // Always save to the latest version
        writer.Write((uint)Version.Latest);
        writer.Write((int)Containers.Count);
        foreach (ArgonClass container in Containers)
        {
            container.Write(writer);
        }
    }

    /// <summary>
    /// Reads a code file
    /// </summary>
    /// <param name="filename">The name of the code file to read.</param>
    /// <returns>New instance of <see cref="ArgonCodeFile"/> containing the read information.</returns>
    public static ArgonCodeFile ReadCodeFile(string filename)
    {
        ArgonCodeFile codeFile = new ArgonCodeFile(filename);

        using FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new BinaryReader(fileStream);

        Version version = (Version)reader.ReadUInt32();
        if (version >= Version.SerializeContainers)
        {
            uint containersCount = reader.ReadUInt32();
            for (int i = 0; i < containersCount; i++)
            {
                codeFile.Containers.Add(ArgonClass.ReadContainer(reader));
            }
        }
        else
        {
            string notUsedMessage = reader.ReadString();
        }

        fileStream.Close();
        reader.Close();

        if (version != Version.Latest)
        {
            codeFile.Save();
        }

        return codeFile;
    }

    /// <summary>
    /// Creates a new <see cref="ArgonCodeFile"/> and saves it to <paramref name="filename"/>.
    /// </summary>
    /// <param name="filename">The location on disk to where this solution should be saved.</param>
    /// <returns>The solution representing the file on disk.</returns>
    public static ArgonCodeFile CreateAndSaveBlank(string filename)
    {
        ArgonCodeFile codeFile = new ArgonCodeFile(filename);
        codeFile.Save();
        return codeFile;
    }
}

public class ArgonClass
{
    public string Name { get; set; }

    public List<ArgonFunctionClassMember> Functions { get; }

    public List<ArgonPropertyClassMember> Properties { get; }

    public ArgonClass(string name) 
    {
        Name = name;
        Functions = new List<ArgonFunctionClassMember>();
        Properties = new List<ArgonPropertyClassMember>();
    }

    public void Write(BinaryWriter writer) 
    {
        writer.Write(Name);

        writer.Write((uint)Functions.Count);
        for (int i = 0; i < Functions.Count; i++)
        {
            Functions[i].Write(writer);
        }

        writer.Write((uint)Properties.Count);
        for (int i = 0; i < Properties.Count; i++)
        {
            Properties[i].Write(writer);
        }
    }

    public static ArgonClass ReadContainer(BinaryReader reader) 
    {
        string containerName = reader.ReadString();
        ArgonClass newContainer = new ArgonClass(containerName);

        uint functionsCount = reader.ReadUInt32();
        for (int i = 0; i < functionsCount; i++)
        {
            newContainer.Functions.Add(ArgonFunctionClassMember.ReadFunction(reader));
        }

        uint propertiesCount = reader.ReadUInt32();
        for (int i = 0; i < functionsCount; i++)
        {
            newContainer.Properties.Add(ArgonPropertyClassMember.ReadProperty(reader));
        }

        return newContainer;
    }
}