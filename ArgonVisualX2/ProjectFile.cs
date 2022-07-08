using System;
using System.IO;

namespace ArgonVisualX2;

public class ProjectFile : ArgonFile
{
    public const string Extension = ".argproj";

    private ProjectFile(FileInfo fileInfo) : base(fileInfo)
    {

    }

    public static ProjectFile Create(string directory, string name)
    {
        string subDirectory = Path.Combine(directory, name);
        Directory.CreateDirectory(subDirectory);

        string filename = Path.Combine(subDirectory, name) + Extension;
        if (File.Exists(filename))
        {
            throw new ArgumentException("Cannot create a solution that already exists");
        }
        return new ProjectFile(new FileInfo(filename));
    }

    public static ProjectFile Read(FileInfo fileInfo)
    {
        ProjectFile solution = new ProjectFile(fileInfo);

        return solution;
    }

    public void Save()
    {

    }
}