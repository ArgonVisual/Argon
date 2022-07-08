using System;
using System.IO;

namespace ArgonVisualX2;

public class CodeFile : ArgonFile
{
    public const string Extension = ".argon";

    private CodeFile(FileInfo fileInfo) : base(fileInfo)
    {

    }

    public static CodeFile Create(string directory, string name)
    {
        string filename = Path.Combine(directory, name) + Extension;
        if (File.Exists(filename))
        {
            throw new ArgumentException("Cannot create a solution that already exists");
        }
        return new CodeFile(new FileInfo(filename));
    }

    public static CodeFile Read(FileInfo fileInfo)
    {
        CodeFile solution = new CodeFile(fileInfo);

        return solution;
    }

    public void Save()
    {

    }
}