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
}