using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArgonUserInterfacePrototype.PinTypes;

namespace ArgonUserInterfacePrototype.Graph.Mars;

public class FunctionNode_Mars : GraphNode
{
    private StackPanel _inputPins;
    private StackPanel _outputPins;

    private ExecutionPin _executionOutPin;
    private ExecutionPin _executionInPin;

    public FunctionNode_Mars(GraphPanel graphPanel) : base(graphPanel)
    {
        StackPanel titlePanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal
        };

        titlePanel.Children.Add(new Image()
        {
            Source = ArgonIcons.FunctionIcon
        });

        titlePanel.Children.Add(new TextBlock()
        {
            Text = "MyFunctionName",
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 20,
            Margin = new Thickness(10, 0, 10, 0)
        });

        this.AddRowFill(new Border()
        {
            Background = BrushHelper.MakeSolidBrush(234, 225, 70),
            Height = 40,
            Child = titlePanel
        });

        Grid pinsPanel = new Grid();

        pinsPanel.AddColumnFill(_inputPins = new StackPanel()
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Center
        });
        pinsPanel.AddColumnFill(_outputPins = new StackPanel()
        {
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center
        });

        _inputPins.Children.Add(new DataPin_Mars(this, "Age", PT_Integer.Instance, GraphPin.PinDirection.Input));
        _inputPins.Children.Add(new DataPin_Mars(this, "Name", PT_String.Instance, GraphPin.PinDirection.Input));

        _outputPins.Children.Add(new DataPin_Mars(this, "Description", PT_String.Instance, GraphPin.PinDirection.Output));

        Grid executionPanel = new Grid();

        executionPanel.AddColumnFill(_executionInPin = new ExecutionPin(this, GraphPin.PinDirection.Input));
        executionPanel.AddColumnFill(_executionOutPin = new ExecutionPin(this, GraphPin.PinDirection.Output));

        this.AddRowAuto(new Border()
        {
            Background = Brushes.White,
            Child = executionPanel
        });

        this.AddRowFill(new Border()
        {
            Background = Brushes.LightGray,
            BorderBrush = Brushes.Gray,
            Child = pinsPanel
        });
    }

    public override IEnumerable<GraphPin> GetOutputPins()
    {
        yield return _executionOutPin;

        foreach (GraphPin outputPin in _outputPins.Children)
        {
            yield return outputPin;
        }
    }

    public override IEnumerable<GraphPin> GetInputPins()
    {
        yield return _executionInPin;

        foreach (GraphPin inputPin in _inputPins.Children)
        {
            yield return inputPin;
        }
    }
}