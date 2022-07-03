using System;
using System.Collections.Generic;
using System.IO;
using Argon.Helpers;
using ArgonVisual.DocumentItems;
using ArgonVisual.Helpers;

namespace ArgonVisual;

/// <summary>
/// Represents a file that contains code.
/// </summary>
public class ArgonCodeFile
{
    /// <summary>
    /// The serialization version for a <see cref="ArgonCodeFile"/>.
    /// </summary>
    public enum Version : byte
    {
        BlankVersion,

        Last,
        Latest = Last - 1
    }

    /// <summary>
    /// The name of this code file
    /// </summary>
    public string Name => FileInfo.Name.SubstringBeforeLast('.');

    /// <summary>
    /// The file on disk that represents this code file.
    /// </summary>
    public FileInfo FileInfo { get; private set; }

    /// <summary>
    /// All the types defined in this file
    /// </summary>
    public List<ArgonUserDefinedType> DefinedTypes { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ArgonCodeFile"/> with <paramref name="fileInfo"/>.
    /// </summary>
    /// <param name="fileInfo">The file info for the code file on disk.</param>
    private ArgonCodeFile(FileInfo fileInfo)
    {
        FileInfo = fileInfo;
        DefinedTypes = new List<ArgonUserDefinedType>();
    }

    public static ArgonCodeFile Create(FileInfo fileInfo)
    {
        ArgonCodeFile newCodeFile = new ArgonCodeFile(fileInfo);
        Save(newCodeFile);
        return newCodeFile;
    }

    /// <summary>
    /// Reads an <see cref="ArgonCodeFile"/> from <paramref name="fileInfo"/>.
    /// </summary>
    /// <param name="fileInfo">The file on disk to read.</param>
    /// <returns>The read code file.</returns>
    /// <exception cref="ArgumentException">The file must exist.</exception>
    public static ArgonCodeFile Read(FileInfo fileInfo)
    {
        if (!fileInfo.Exists)
        {
            throw new ArgumentException("Codo file to read must exist", nameof(fileInfo));
        }

        ArgonCodeFile codeFile = new ArgonCodeFile(fileInfo);

        Version version = Version.Latest;

        using (FileStream fileStream = fileInfo.OpenRead())
        {
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                version = (Version)binaryReader.ReadByte();

                binaryReader.ReadArray(codeFile.DefinedTypes, () => ArgonUserDefinedType.Read(binaryReader));
            }
        }

        if (version != Version.Latest)
        {
            Save(codeFile);
        }

        return codeFile;
    }

    /// <summary>
    /// Saves the <see cref="ArgonCodeFile"/> to disk.
    /// </summary>
    /// <param name="codeFile">The <see cref="ArgonCodeFile"/> to save.</param>
    public static void Save(ArgonCodeFile codeFile)
    {
        using (FileStream fileStream = codeFile.FileInfo.OpenWrite())
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.Write((byte)Version.Latest);
                binaryWriter.WriteArray(codeFile.DefinedTypes, (item) => item.Write(binaryWriter));
            }
        }
    }

    /// <summary>
    /// Renames the code file
    /// </summary>
    /// <param name="newName">The new name of the file without the extension</param>
    public void Rename(string newName) 
    {
        FileInfo = new FileInfo(PathHelper.RenameFile(FileInfo.FullName, newName + ArgonFileExtensions.CodeFile));
    }
}
