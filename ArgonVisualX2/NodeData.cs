using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace ArgonVisualX2;

public class NodeData
{
    public EditableText? VisualNameText { get; set; }

    public string Name 
    { 
        get; 
        set; 
    }

    public Point Position { get; set; }

    public bool InlinesNeedPopulating { get; private set; }

    private StringBuilder _nameBuilder;

    public NodeData(string name)
    {
        Name = name;
        InlinesNeedPopulating = true;
        _nameBuilder = new StringBuilder();
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

    public void PopulateInlines() 
    {
        // if (!InlinesNeedPopulating || Inlines is null)
        // {
        //     return;
        // }
        // 
        // Inlines.Clear();
        // InlinesNeedPopulating = false;
        // 
        // if (Name.Length > 0)
        // {
        //     bool isReadingParameter = false;
        // 
        //     for (int i = 0; i < Name.Length; i++)
        //     {
        //         char character = Name[i];
        //         if (character == '{' && !isReadingParameter)
        //         {
        //             if (_nameBuilder.Length > 0)
        //             {
        //                 Inlines.Add(new Run(_nameBuilder.ToString())
        //                 {
        //                     BaselineAlignment = BaselineAlignment.Center
        //                 });
        //                 _nameBuilder.Clear();
        //             }
        //             isReadingParameter = true;
        //         }
        //         else if (isReadingParameter && character == '}')
        //         {
        //             Inlines.Add(new InlineUIContainer(new ParameterVisual() { ParameterName = _nameBuilder.ToString() })
        //             {
        //                 BaselineAlignment = BaselineAlignment.Center
        //             });
        //             _nameBuilder.Clear();
        //             isReadingParameter = false;
        //         }
        //         else
        //         {
        //             _nameBuilder.Append(character);
        //         }
        //     }
        // 
        //     if (isReadingParameter)
        //     {
        //         Inlines.Add(new InlineUIContainer(new ParameterVisual() { ParameterName = _nameBuilder.ToString() })
        //         {
        //             BaselineAlignment = BaselineAlignment.Center
        //         });
        //     }
        //     else
        //     {
        //         Inlines.Add(new Run(_nameBuilder.ToString())
        //         {
        //             BaselineAlignment = BaselineAlignment.Center
        //         });
        //     }
        // 
        //     _nameBuilder.Clear();
        // }
    }
}

