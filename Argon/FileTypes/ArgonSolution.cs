using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using Argon.Helpers;
using Argon.Widgets;
using static Argon.Helpers.StringHelper;

namespace Argon.FileTypes;

/// <summary>
/// Represents an Argon solution file in memory. The file extension is <see cref="FileExtensions.Solution"/>.
/// A solution contains one or many <see cref="ArgonProject"/>s.
/// </summary>
public class ArgonSolution : IFileHandle
{
    /// <summary>
    /// The serialization version
    /// </summary>
    public enum Version : uint
    {
        Dev_FirstSolutionVersion,
        AddedReferencedProjects,
        SolutionDirectories,

        Last,
        Latest = Last - 1,
    }

    /// <summary>
    /// The full name of the solution on disk.
    /// </summary>
    public string Filename => Path.Combine(Directory, Name) + FileExtensions.Solution;

    /// <summary>
    /// The name of the solution - the name of the file without directory and extension
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The directory that this solution is contained inside of.
    /// </summary>
    public string Directory { get; }

    /// <summary>
    /// List of projects that are placed directy inside of the solution.
    /// </summary>
    public List<ArgonProject> SolutionProjects { get; }

    /// <summary>
    /// Directories that are contained directly inside of this solution.
    /// These directories can contain projects
    /// </summary>
    public List<SolutionDirectory> SolutionDirectories { get; }

    public ArgonSolution(string filename)
    {
        (Directory, Name) = filename.GetDirectoryAndName();

        SolutionProjects = new List<ArgonProject>();
        SolutionDirectories = new List<SolutionDirectory>();
    }

    /// <summary>
    /// Reads a solution file
    /// </summary>
    /// <param name="filename">The name of the solution file to read.</param>
    /// <returns>New instance of <see cref="ArgonSolution"/> containing the read information.</returns>
    public static ArgonSolution ReadSolution(string filename)
    {
        ArgonSolution solution = new ArgonSolution(filename);

        using FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new BinaryReader(fileStream);
        Version version = (Version)reader.ReadUInt32();

        if (version >= Version.AddedReferencedProjects)
        {
            // Serializes the projects that are directly contained inside of the solution
            uint projectsCount = reader.ReadUInt32();
            for (int i = 0; i < projectsCount; i++)
            {
                string projectFilename = reader.ReadString();
                if (File.Exists(projectFilename))
                {
                    solution.SolutionProjects.Add(ArgonProject.ReadProject(projectFilename));
                }
            }
        }

        // Serialize the sub directories in the solution and the projects that they contain
        if (version >= Version.SolutionDirectories)
        {
            uint directoriesCount = reader.ReadUInt32();
            for (int i = 0; i < directoriesCount; i++)
            {
                string directoryName = reader.ReadString();
                SolutionDirectory solutionDirectory = new SolutionDirectory(new DirectoryInfo(directoryName));
                solutionDirectory.Read(reader);
                solution.SolutionDirectories.Add(solutionDirectory);
            }
        }

        fileStream.Close();
        reader.Close();

        if (version != Version.Latest)
        {
            // Save the solution to the latest serialization version
            solution.Save();
        }

        return solution;
    }

    /// <summary>
    /// Saves this to a directory.
    /// </summary>
    /// <param name="directory">The directory to save this solution to.</param>
    public void Save()
    {
        using FileStream fileStream = new FileStream(Filename, FileMode.Create, FileAccess.Write);
        using BinaryWriter writer = new BinaryWriter(fileStream);
        writer.Write((uint)Version.Latest);

        // Save the projects that are contained directly inside of the solution
        writer.Write((uint)SolutionProjects.Count);
        for (int i = 0; i < SolutionProjects.Count; i++)
        {
            writer.Write(SolutionProjects[i].Filename);
        }

        writer.Write((uint)SolutionDirectories.Count);
        for (int i = 0; i < SolutionDirectories.Count; i++)
        {
            SolutionDirectory solutionDirectory = SolutionDirectories[i];
            writer.Write(solutionDirectory.Directory.FullName);
            solutionDirectory.Write(writer);
        }

        fileStream.Close();
        writer.Close();
    }

    /// <summary>
    /// Creates a new <see cref="ArgonSolution"/> and saves it to <paramref name="filename"/>.
    /// </summary>
    /// <param name="filename">The location on disk to where this solution should be saved.</param>
    /// <returns>The solution representing the file on disk.</returns>
    public static ArgonSolution CreateAndSaveBlank(string filename)
    {
        ArgonSolution solution = new ArgonSolution(filename);
        solution.Save();
        return solution;
    }

    /// <summary>
    /// Creates a new window containing a <see cref="SolutionEditor"/> for this solution. 
    /// </summary>
    public Window CreateSolutionEditorWindow()
    {
        Window solutionWindow = SolutionEditor.CreateWindow(this);
        Application.Current.MainWindow = solutionWindow;
        return solutionWindow;
    }

    public List<ArgonProject> GetProjectListForDirectory(string directory) 
    {
        if (Directory == directory)
        {
            return SolutionProjects;
        }

        return GetSolutionDirectory(new DirectoryInfo(directory)).Projects;
    }

    public SolutionDirectory GetSolutionDirectory(DirectoryInfo directory) 
    {
        foreach (SolutionDirectory solutionDirectory in SolutionDirectories)
        {
            SolutionDirectory? foundDirectory = solutionDirectory.GetSolutionDirectory(directory);
            if (foundDirectory is not null)
            {
                return foundDirectory;
            }
        }

        throw new Exception($"{directory.FullName} does not exist in solution");
    }
}

/// <summary>
/// Represents a directory that is contained directory inside of a solution.
/// This directory can contain projects.
/// </summary>
public class SolutionDirectory 
{
    /// <summary>
    /// The directory on disk that this represents.
    /// </summary>
    public DirectoryInfo Directory { get; 
        set; /* Supports renaming of folder */ }

    /// <summary>
    /// The projects if any that are contained inside of this directory.
    /// </summary>
    public List<ArgonProject> Projects { get; }

    /// <summary>
    /// The directories that are contained inside of this directory.
    /// </summary>
    public List<SolutionDirectory> SubDirectories { get; }

    public SolutionDirectory(DirectoryInfo directory) 
    {
        Directory = directory;
        Projects = new List<ArgonProject>();
        SubDirectories = new List<SolutionDirectory>();
    }

    public void Read(BinaryReader reader) 
    {
        uint projectsCount = reader.ReadUInt32();
        for (int i = 0; i < projectsCount; i++)
        {
            string projectFilename = reader.ReadString();
            Projects.Add(ArgonProject.ReadProject(projectFilename));
        }

        uint subDirectoriesCount = reader.ReadUInt32();
        for (int i = 0; i < subDirectoriesCount; i++)
        {
            string subDirectoryFullName = reader.ReadString();
            SolutionDirectory solutionDirectory = new SolutionDirectory(new DirectoryInfo(subDirectoryFullName));
            solutionDirectory.Read(reader);
            SubDirectories.Add(solutionDirectory);
        }
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write((uint)Projects.Count);
        for (int i = 0; i < Projects.Count; i++)
        {
            writer.Write(Projects[i].Filename);
        }

        writer.Write((uint)SubDirectories.Count);
        for (int i = 0; i < SubDirectories.Count; i++)
        {
            writer.Write(SubDirectories[i].Directory.FullName);
            SubDirectories[i].Write(writer);
        }
    }

    public SolutionDirectory? GetSolutionDirectory(DirectoryInfo directory) 
    {
        if (EnsureDoesNotEndWithSlash(Directory.FullName) == EnsureDoesNotEndWithSlash(directory.FullName))
        {
            return this;
        }

        foreach (SolutionDirectory subDirectory in SubDirectories)
        {
            return subDirectory.GetSolutionDirectory(directory);
        }

        return null;
    }

    public static string EnsureDoesNotEndWithSlash(string directory) 
    {
        if (directory.EndsWith(Path.DirectorySeparatorChar))
        {
            return directory.Substring(0, directory.Length - 1);
        }
        else
        {
            return directory;
        }
    }
}