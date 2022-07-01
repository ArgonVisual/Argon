using System;
using System.IO;
using System.Windows.Controls;
using Argon.Helpers;

namespace ArgonVisual;

/// <summary>
/// Represents a project.
/// A project can be referenced by a solution.
/// A project can be compiled into a executable or a library.
/// </summary>
public class ArgonProject 
{
    /// <summary>
    /// The serialization version for a <see cref="ArgonProject"/>.
    /// </summary>
    public enum Version
    {
        BlankVersion,

        Last,
        Latest = Last - 1
    }

    public string Name => FileInfo.Name.SubstringBeforeLast('.');

    /// <summary>
    /// The file on disk that represents this project.
    /// </summary>
    public FileInfo FileInfo { get; }

    public TreeView? TreeView { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="ArgonProject"/> with <paramref name="fileInfo"/>.
    /// </summary>
    /// <param name="fileInfo">The file info for the project on disk.</param>
    private ArgonProject(FileInfo fileInfo) 
    {
        FileInfo = fileInfo;
    }

    /// <summary>
    /// Creates a new <see cref="ArgonProject"/> on disk at <see cref="FileInfo"/> and references it in <paramref name="ownerSolution"/>.
    /// </summary>
    /// <param name="fileInfo">The location to where this project should be located.</param>
    /// <param name="ownerSolution">The solution that owns this project.</param>
    /// <returns>The new <see cref="ArgonProject"/>.</returns>
    public static ArgonProject Create(FileInfo fileInfo) 
    {
        ArgonProject newProject = new ArgonProject(fileInfo);
        Save(newProject);
        return newProject;
    }

    /// <summary>
    /// Reads an <see cref="ArgonProject"/> from <paramref name="fileInfo"/>.
    /// </summary>
    /// <param name="fileInfo">The file on disk to read.</param>
    /// <returns>The read project</returns>
    /// <exception cref="ArgumentException">The file must exist.</exception>
    public static ArgonProject Read(FileInfo fileInfo) 
    {
        if (!fileInfo.Exists)
        {
            throw new ArgumentException("Project to read must exist", nameof(fileInfo));
        }

        ArgonProject newProject = new ArgonProject(fileInfo);

        using (FileStream fileStream = fileInfo.OpenRead())
        {
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {

            }
        }

        return newProject;
    }

    public static void Save(ArgonProject project) 
    {
        using (FileStream fileStream = project.FileInfo.OpenWrite())
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            {

            }
        }
    }

    public void Rename(string newName) 
    {
        throw new NotImplementedException();
    }

    public string GetRelativePathToSolution(ArgonSolution solution) 
    {
        if (solution.FileInfo.Directory is null)
        {
            throw new ArgumentException("Solution must have a parent directory", nameof(solution));
        }

        return Path.GetRelativePath(solution.FileInfo.Directory.FullName, FileInfo.FullName);
    }
}