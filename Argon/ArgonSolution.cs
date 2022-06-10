using System;
using System.IO;
using System.Windows;
using Argon.Helpers;
using Argon.Widgets;
using static Argon.Helpers.StringHelper;

namespace Argon;

/// <summary>
/// Represents an Argon solution file in memory. The file extension is <see cref="FileExtensions.Solution"/>.
/// A solution contains one or many <see cref="ArgonProject"/>s.
/// </summary>
public class ArgonSolution
{
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

    public ArgonSolution(string filename) 
    {
        (string directory, string name) = PathHelper.GetDirectoryAndName(filename);
        Name = name;
        Directory = directory;
    }

    /// <summary>
    /// Reads a solution file
    /// </summary>
    /// <param name="filename">The name of the solution file to read.</param>
    /// <returns>New instance of <see cref="ArgonSolution"/> containing the read information.</returns>
    public static ArgonSolution ReadSolution(string filename) 
    {
        string name = Path.GetFileName(filename);
        ArgonSolution solution = new ArgonSolution(filename);

        using FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        using BinaryReader writer = new BinaryReader(fileStream);
        string message = writer.ReadString();

        return solution;
    }

    /// <summary>
    /// Saves this to a directory.
    /// </summary>
    /// <param name="directory">The directory to save this solution to.</param>
    public void Save(string directory) 
    {
        using FileStream fileStream = new FileStream(Path.Combine(directory, Name) + FileExtensions.Solution, FileMode.CreateNew, FileAccess.Write);
        using BinaryWriter writer = new BinaryWriter(fileStream);
        writer.Write("Hello World");
    }

    /// <summary>
    /// Creates a new <see cref="ArgonSolution"/> and saves it to <paramref name="filename"/>.
    /// </summary>
    /// <param name="filename">The location on disk to where this solution should be saved.</param>
    /// <returns>The solution representing the file on disk.</returns>
    public static ArgonSolution CreateAndSaveBlank(string filename) 
    {
        ArgonSolution solution = new ArgonSolution(filename);
        solution.Save(filename.SubstringBeforeLast(Path.DirectorySeparatorChar));
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
}