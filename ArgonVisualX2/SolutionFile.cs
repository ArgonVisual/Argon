using System;
using System.IO;

namespace ArgonVisualX2;

public class SolutionFile : ArgonFile
{
    public const string Extension = ".argsln";

    private SolutionFile(FileInfo fileInfo) : base(fileInfo)
    {

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
        return new SolutionFile(new FileInfo(filename));
    }

    public static SolutionFile Read(FileInfo fileInfo) 
    {
        SolutionFile solution = new SolutionFile(fileInfo);

        return solution;
    }

    public void Save()
    {
        
    }
}