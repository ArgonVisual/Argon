using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using static System.Linq.Enumerable;

namespace ArgonVisualX2;

public class SolutionFile : ArgonFile
{
    public const string Extension = ".argsln";

    public List<ProjectFile> Projects { get; }

    private SolutionFile(FileInfo fileInfo) : base(fileInfo)
    {
        Projects = new List<ProjectFile>();
    }

    public static SolutionFile Create(string directory, string name) 
    {
        string subDirectory = Path.Combine(directory, name);
        Directory.CreateDirectory(subDirectory);

        string filename = Path.Combine(subDirectory, name) + Extension;
        if (File.Exists(filename))
        {
            throw new ArgumentException("Cannot create a solution that already exists");
        }

        SolutionFile solution = new SolutionFile(new FileInfo(filename));
        ProjectFile project = ProjectFile.Create(subDirectory, name);
        solution.Projects.Add(project);
        solution.Save();

        return solution;
    }

    public static SolutionFile Read(FileInfo fileInfo)
    {
        return ReadStatic(new SolutionFile(fileInfo));
    }

    protected override void WriteInternal(ArgonBinaryWriter writer)
    {
        writer.WriteArray(Projects, (project) => project.WriteForSolution(writer));
    }

    protected override void ReadInternal(ArgonBinaryReader reader)
    {
        reader.ReadArray(Projects, () => ProjectFile.ReadForSolution(reader));
    }
}