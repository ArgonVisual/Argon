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
        CodeFile codeFile = new CodeFile(new FileInfo(filename));
        codeFile.Save();
        return codeFile;
    }

    public static CodeFile Read(FileInfo fileInfo)
    {
        return ReadStatic(new CodeFile(fileInfo));
    }

    protected override void WriteInternal(ArgonBinaryWriter writer)
    {

    }

    protected override void ReadInternal(ArgonBinaryReader writer)
    {

    }
}