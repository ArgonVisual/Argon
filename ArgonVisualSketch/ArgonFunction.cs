using System.IO;

namespace ArgonVisual;

public class ArgonFunction
{
    public string Name { get; set; }

    public ArgonFunction(string name) 
    {
        Name = name;
    }

    public static ArgonFunction Read(BinaryReader reader)
    {
        string functionName = reader.ReadString();
        ArgonFunction function = new ArgonFunction(functionName);
        return function;
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Name);
    }
}
