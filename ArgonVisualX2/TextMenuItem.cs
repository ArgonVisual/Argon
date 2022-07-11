using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArgonVisualX2;

public class TextMenuItem : MenuItem
{
    public TextMenuItem(string name, Action onClicked)
    {
        FontSize = 20;
        Header = name;
        Click += (sender, args) => onClicked();
    }
}
