using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Argon.Helpers;

namespace ArgonVisual;

/// <summary>
/// Represents a file that contains code.
/// </summary>
public class ArgonCodeFile
{
    /// <summary>
    /// The serialization version for a <see cref="ArgonCodeFile"/>.
    /// </summary>
    public enum Version
    {
        BlankVersion,

        Last,
        Latest = Last - 1
    }

    public string Name => FileInfo.Name.SubstringBeforeLast('.');

    /// <summary>
    /// The file on disk that represents this code file.
    /// </summary>
    public FileInfo FileInfo { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="ArgonCodeFile"/> with <paramref name="fileInfo"/>.
    /// </summary>
    /// <param name="fileInfo">The file info for the code file on disk.</param>
    private ArgonCodeFile(FileInfo fileInfo)
    {
        FileInfo = fileInfo;
    }

    public static ArgonCodeFile Create(FileInfo fileInfo)
    {
        ArgonCodeFile newProject = new ArgonCodeFile(fileInfo);
        Save(newProject);
        return newProject;
    }

    /// <summary>
    /// Reads an <see cref="ArgonCodeFile"/> from <paramref name="fileInfo"/>.
    /// </summary>
    /// <param name="fileInfo">The file on disk to read.</param>
    /// <returns>The read project</returns>
    /// <exception cref="ArgumentException">The file must exist.</exception>
    public static ArgonCodeFile Read(FileInfo fileInfo)
    {
        if (!fileInfo.Exists)
        {
            throw new ArgumentException("Codo file to read must exist", nameof(fileInfo));
        }

        ArgonCodeFile newCodeFile = new ArgonCodeFile(fileInfo);

        using (FileStream fileStream = fileInfo.OpenRead())
        {
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {

            }
        }

        return newCodeFile;
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

            }
        }
    }

    /// <summary>
    /// Gets the relative path from <paramref name="solution"/> to this project.
    /// </summary>
    /// <param name="solution">The solution to get the relative path from.</param>
    /// <returns>The relative path.</returns>
    /// <exception cref="ArgumentException">The solution must have a parent directory.</exception>
    public string GetRelativePathToSolution(ArgonSolution solution)
    {
        if (solution.FileInfo.Directory is null)
        {
            throw new ArgumentException("Solution must have a parent directory", nameof(solution));
        }

        return Path.GetRelativePath(solution.FileInfo.Directory.FullName, FileInfo.FullName);
    }
}
