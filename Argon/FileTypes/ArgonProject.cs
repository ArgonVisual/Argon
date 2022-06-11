using System.IO;
using Argon.Helpers;

namespace Argon.FileTypes;

/// <summary>
/// Represents a project that can be compiled.
/// </summary>
public class ArgonProject : IFileHandle
{
    /// <summary>
    /// The serialization version for the binary format
    /// </summary>
    public enum Version : uint
    {
        Dev_First,

        Last,
        Latest = Last - 1,
    }

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

    /// <summary>
    /// Initializes a new instance of <see cref="ArgonProject"/>.
    /// </summary>
    /// <param name="filename">The path on disk to where this project should be located.</param>
    public ArgonProject(string filename)
    {
        (Directory, Name) = filename.GetDirectoryAndName();
    }

    /// <summary>
    /// Saves this project on disk
    /// </summary>
    public void Save()
    {
        using FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write);
        using BinaryWriter writer = new BinaryWriter(fileStream);

        // Write the serialization version of the file
        // Always save to the latest version
        writer.Write((uint)Version.Latest);
        writer.Write("This is an argon project");
    }

    /// <summary>
    /// Reads a project file
    /// </summary>
    /// <param name="filename">The name of the project file to read.</param>
    /// <returns>New instance of <see cref="ArgonProject"/> containing the read information.</returns>
    public static ArgonProject ReadProject(string filename)
    {
        ArgonProject solution = new ArgonProject(filename);

        using FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new BinaryReader(fileStream);

        // Read the serialization version of the fil
        Version version = (Version)reader.ReadUInt32();
        string message = reader.ReadString();

        return solution;
    }

    /// <summary>
    /// Creaes a new blank <see cref="ArgonProject"/> and saves it to <paramref name="filename"/>.
    /// </summary>
    /// <param name="filename">The location on disk of this project (extension is not needed)</param>
    /// <returns>The new project.h</returns>
    public static ArgonProject CreateAndSaveBlank(string directory, string name)
    {
        string newDirectoryPath = Path.Combine(directory, name);
        System.IO.Directory.CreateDirectory(newDirectoryPath);
        ArgonProject newProject = new ArgonProject(Path.Combine(newDirectoryPath, name));
        newProject.Save();
        return newProject;
    }

    public void MarkForSave()
    {
        Argon.MarkFileForSave(this);
    }
}