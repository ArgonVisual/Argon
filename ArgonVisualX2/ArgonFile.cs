using System.IO;
using static ArgonVisualX2.Helpers.StringHelper;

namespace ArgonVisualX2;

public abstract class ArgonFile 
{
    public string Name => FileInfo.Name.SubstringBefore('.');

    public FileInfo FileInfo { get; private set; }

    protected ArgonFile(FileInfo fileInfo) 
    {
        FileInfo = fileInfo;
    }

    public void Save() 
    {
        FileStream stream = FileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write);
        ArgonBinaryWriter writer = new ArgonBinaryWriter(stream);

        WriteInternal(writer);

        writer.Close();
        stream.Close();
    }

    protected static T ReadStatic<T>(T file) where T : ArgonFile 
    {
        FileStream stream = file.FileInfo.Open(FileMode.Open, FileAccess.Read);
        ArgonBinaryReader reader = new ArgonBinaryReader(stream);

        file.ReadInternal(reader);

        reader.Close();
        stream.Close();

        return file;
    }

    protected abstract void WriteInternal(ArgonBinaryWriter writer);

    protected abstract void ReadInternal(ArgonBinaryReader reader);
}