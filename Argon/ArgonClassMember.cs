using System.IO;

namespace Argon;

public abstract class ArgonClassMember 
{
    public string Name { get; set; }

    public ArgonClassMember(string name) 
    {
        Name = name;
    }

    public virtual void Write(BinaryWriter writer)
    {
        writer.Write(Name);
    }
}

public class ArgonFunctionClassMember : ArgonClassMember
{
    public ArgonFunctionClassMember(string name) : base(name)
    {

    }

    public static ArgonFunctionClassMember ReadFunction(BinaryReader reader)
    {
        string name = reader.ReadString();
        ArgonFunctionClassMember function = new ArgonFunctionClassMember(name);
        return function;
    }
}

public class ArgonPropertyClassMember : ArgonClassMember
{
    public ArgonPropertyClassMember(string name) : base(name)
    {

    }

    public static ArgonPropertyClassMember ReadProperty(BinaryReader reader)
    {
        string name = reader.ReadString();
        ArgonPropertyClassMember property = new ArgonPropertyClassMember(name);
        return property;
    }
}