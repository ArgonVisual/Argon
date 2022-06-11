using System.IO;
using Argon.Helpers;

namespace Argon.FileTypes;

/// <summary>
/// Represents a file in memory that contains code.
/// </summary>
public class ArgonCodeFile // : ISaveable
{
    /// <summary>
    /// The full name of the project on disk.
    /// </summary>
    public string Filename => Path.Combine(Directory, Name) + FileExtensions.Project;

    /// <summary>
    /// The name of the project
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The directory on disk that the project is contained inside of.
    /// </summary>
    public string Directory { get; }

    public ArgonCodeFile(string filename)
    {
        (Directory, Name) = filename.GetDirectoryAndName();
    }
}