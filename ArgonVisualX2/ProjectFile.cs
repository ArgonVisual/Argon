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
        ProjectFile project = new ProjectFile(new FileInfo(filename));
        project.Save();
        return project;
    }

    public static ProjectFile Read(FileInfo fileInfo)
    {
        return ReadStatic(new ProjectFile(fileInfo));
    }

    public void WriteForSolution(ArgonBinaryWriter writer) 
    {
        writer.Write(FileInfo.FullName);
    }

    public static ProjectFile ReadForSolution(ArgonBinaryReader reader) 
    {
        string filename = reader.ReadString();
        return ProjectFile.Read(new FileInfo(filename));
    }

    protected override void WriteInternal(ArgonBinaryWriter writer)
    {

    }

    protected override void ReadInternal(ArgonBinaryReader writer)
    {

    }
}