using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArgonVisualX2;
/// <summary>
/// Interaction logic for NodeVisual.xaml
/// </summary>
public partial class NodeVisual : UserControl
{
    private string _nodeTitle;

    public string NodeTitle 
    {
        get => _nodeTitle;
        set
        {
            _nodeTitle = value;
            PopulateInlines();
        }
    }

    public NodeVisual()
    {
        InitializeComponent();
    }

    private void PopulateInlines() 
    {
        bool isReadingParameter = false;

        StringBuilder builder = new StringBuilder();

        TitleText.Inlines.Clear();

        for (int i = 0; i < _nodeTitle.Length; i++)
        {
            char character = _nodeTitle[i];
            if (character == '{' && !isReadingParameter)
            {
                if (builder.Length > 0)
                {
                    TitleText.Inlines.Add(new Run(builder.ToString())
                    {
                        BaselineAlignment = BaselineAlignment.Center
                    });
                    builder.Clear();
                }
                isReadingParameter = true;
            }
            else if (isReadingParameter && character == '}')
            {
                TitleText.Inlines.Add(new InlineUIContainer(new ParameterVisual() { ParameterName = builder.ToString() })
                {
                    BaselineAlignment = BaselineAlignment.Center
                });
                builder.Clear();
                isReadingParameter = false;
            }
            else
            {
                builder.Append(character);
            }
        }

        if (isReadingParameter)
        {
            TitleText.Inlines.Add(new InlineUIContainer(new ParameterVisual() { ParameterName = builder.ToString() })
            {
                BaselineAlignment = BaselineAlignment.Center
            });
        }
        else
        {
            TitleText.Inlines.Add(new Run(builder.ToString())
            {
                BaselineAlignment = BaselineAlignment.Center
            });
        }

        builder.Clear();
    }
}
