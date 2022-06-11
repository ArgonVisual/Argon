using System;
using System.Collections.Generic;
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
    /// List of projects that are referenced in this solution.
    /// </summary>
    public List<ArgonProject> Projects { get; }

    public ArgonSolution(string filename)
    {
        (Directory, Name) = filename.GetDirectoryAndName();

        Projects = new List<ArgonProject>();
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
            uint projectsCount = reader.ReadUInt32();
            for (int i = 0; i < projectsCount; i++)
            {
                string projectFilename = reader.ReadString();
                if (File.Exists(projectFilename))
                {
                    solution.Projects.Add(ArgonProject.ReadProject(projectFilename));
                }
            }
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
        writer.Write((uint)Projects.Count);
        for (int i = 0; i < Projects.Count; i++)
        {
            writer.Write(Projects[i].Filename);
        }
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

    public void MarkForSave()
    {
        Argon.MarkFileForSave(this);
    }
}