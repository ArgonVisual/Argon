using System.IO;

namespace ArgonVisual.DocumentItems;

public class ArgonUserDefinedType
{
    public string Name { get; set; }

    public ArgonUserDefinedType(string name) 
    {
        Name = name;
    }

    public static ArgonUserDefinedType Read(BinaryReader reader) 
    {
        string typename = reader.ReadString();
        ArgonUserDefinedType userDefinedType = new ArgonUserDefinedType(typename);
        return userDefinedType;
    }

    public void Write(BinaryWriter writer) 
    {
        writer.Write(Name);
    }
}
