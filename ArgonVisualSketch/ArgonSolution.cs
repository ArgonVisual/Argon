using System;
using System.Collections.Generic;
using System.IO;
using Argon.Helpers;

namespace ArgonVisual;

/// <summary>
/// Represents a solution on disk.
/// </summary>
public class ArgonSolution
{
    /// <summary>
    /// The serialization version for a <see cref="ArgonProject"/>.
    /// </summary>
    public enum Version
    {
        BlankVersion,
        RelativePathsToProjects,

        Last,
        Latest = Last - 1
    }

    /// <summary>
    /// The name of the solution without the extension.
    /// </summary>
    public string Name => FileInfo.Name.SubstringBeforeLast('.');

    /// <summary>
    /// The file on disk that represents this project.
    /// </summary>
    public FileInfo FileInfo { get; }

    /// <summary>
    /// The projects that this solution references.
    /// Use <see cref="AddProject(ArgonProject)"/> and <see cref="RemoveProject(ArgonProject)"/> to modify this.
    /// </summary>
    public IReadOnlyList<ArgonProject> Projects => _projects;

    private List<ArgonProject> _projects;

    /// <summary>
    /// Initializes a new instance of <see cref="ArgonSolution"/> with <paramref name="fileInfo"/>.
    /// </summary>
    /// <param name="fileInfo">The file info for the project on disk.</param>
    private ArgonSolution(FileInfo fileInfo)
    {
        FileInfo = fileInfo;
        _projects = new List<ArgonProject>();
    }

    /// <summary>
    /// Creates a new <see cref="ArgonSolution"/> and saves it to disk.
    /// </summary>
    /// <param name="fileInfo">The location on disk of the new solution.</param>
    /// <returns>The new solution</returns>
    public static ArgonSolution Create(DirectoryInfo directory, string name) 
    {
        ArgonSolution newSolution = new ArgonSolution(new FileInfo(Path.Combine(directory.FullName, name) + ArgonFileExtensions.Soluion));
        Save(newSolution);
        return newSolution;
    }

    /// <summary>
    /// Reads an <see cref="ArgonSolution"/> from <paramref name="fileInfo"/>.
    /// </summary>
    /// <param name="fileInfo">The file on disk to read.</param>
    /// <returns>The read project</returns>
    /// <exception cref="ArgumentException">The file must exist.</exception>
    public static ArgonSolution Read(FileInfo fileInfo)
    {
        if (!fileInfo.Exists)
        {
            throw new ArgumentException("Project to read must exist", nameof(fileInfo));
        }

        ArgonSolution solution = new ArgonSolution(fileInfo);

        Version version = Version.Latest;

        using (FileStream fileStream = fileInfo.OpenRead())
        {
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                version = (Version)binaryReader.ReadByte();

                byte projectsCount = binaryReader.ReadByte();
                string rootPath = fileInfo.Directory.FullName;
                
                for (int i = 0; i < projectsCount; i++)
                {
                    string relativePath = binaryReader.ReadString();
                    solution._projects.Add(ArgonProject.Read(new FileInfo(Path.GetFullPath(relativePath, rootPath))));
                }
            }
        }

        if (version != Version.Latest)
        {
            Save(solution);
        }

        return solution;
    }

    /// <summary>
    /// Saves a <see cref="ArgonSolution"/>.
    /// </summary>
    /// <param name="solution">The solution to save.</param>
    public static void Save(ArgonSolution solution)
    {
        using (FileStream fileStream = solution.FileInfo.OpenWrite())
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.Write((byte)Version.Latest);

                binaryWriter.Write((byte)solution._projects.Count);
                for (int i = 0; i < solution._projects.Count; i++)
                {
                    binaryWriter.Write(solution._projects[i].GetRelativePathToSolution(solution));
                }
            }
        }
    }

    /// <summary>
    /// Adds a project to be referenced in this solution.
    /// </summary>
    /// <param name="project">The project to add.</param>
    public void AddProject(ArgonProject project) 
    {
        _projects.Add(project);
        Save(this);
    }

    /// <summary>
    /// Removes a project to be referenced in this solution.
    /// </summary>
    /// <param name="project">The project to remove.</param>
    public void RemoveProject(ArgonProject project) 
    {
        _projects.Remove(project);
        Save(this);
    }
}
