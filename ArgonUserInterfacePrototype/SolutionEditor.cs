using System.Windows.Controls;
using ArgonUserInterfacePrototype.Graph.Mars;
using ArgonUserInterfacePrototype.Graph.Neptune;

namespace ArgonUserInterfacePrototype;

public class SolutionEditor : ContentControl
{
    public SolutionEditor() 
    {
        Grid mainGrid = new Grid();

        mainGrid.AddColumnPixel(250, new SolutionExplorer());
        mainGrid.AddColumnFill(new GraphPanel_Neptune());

        Content = mainGrid;
    }
}
