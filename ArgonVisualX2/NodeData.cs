using System.Windows;

namespace ArgonVisualX2;

public class NodeData
{
    public string Name { get; set; }

    public Point Position { get; set; }

    public NodeData(string name) 
    {
        Name = name;
    }

    public void Write(ArgonBinaryWriter writer) 
    {
        writer.Write(Name);
        writer.Write((int)Position.X);
        writer.Write((int)Position.Y);
    }

    public static NodeData Read(ArgonBinaryReader reader) 
    {
        string name = reader.ReadString();

        NodeData node = new NodeData(name);

        int x = reader.ReadInt32();
        int y = reader.ReadInt32();
        node.Position = new Point(x, y);

        return node;
    }
}