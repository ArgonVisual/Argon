using System;
using System.Net;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ArgonUserInterfacePrototype;

namespace ArgonVertical.Static;

public class GraphPanel : Canvas
{
    private static GraphPanel? _global;

    public static GraphNode? CurrentDraggedNode;

    public static GraphPanel Global 
    { 
        get => _global ?? throw new NullReferenceException("GraphPanel has not been instanced!");
        set => _global = value; 
    }

    public GraphPanel()
    {
        _global = this;

        Background = BrushHelper.MakeSolidBrush(210, 210, 210);

        ContextMenu contextMenu = new ContextMenu();

        MenuItem addFunctionItem = new MenuItem()
        {
            Header = "Add Function"
        };
        addFunctionItem.Click += CreateNewFunction;
        contextMenu.Items.Add(addFunctionItem);

        ContextMenu = contextMenu;

        GraphNode newNode = new DefineFunctionNode("Start Program", null);
        Canvas.SetTop(newNode, 25);
        Canvas.SetLeft(newNode, 25);
        Children.Add(newNode);
    }

    private void CreateNewFunction(object sender, RoutedEventArgs e)
    {
        Point mousePosition = Mouse.GetPosition(this);
        GraphNode newNode = new DefineFunctionNode("My Function", null);
        Canvas.SetTop(newNode, mousePosition.Y);
        Canvas.SetLeft(newNode, mousePosition.X);
        Children.Add(newNode);
    }
}

public abstract class GraphNode : Border
{
    private NodeTitle _nodeTitle;

    private static GraphNode? _hoveredNode;

    protected StackPanel MainPanel;

    private GraphNode? _parentNode;

    public virtual bool IsNotRootNode => true;

    private bool _isDirectlyHovered;

    public GraphNode(string title, GraphNode? parent)
    {
        MainPanel = new StackPanel();
        _parentNode = parent;
        Background = _normalBackground;
        HorizontalAlignment = HorizontalAlignment.Left;

        BorderBrush = BrushHelper.MakeSolidBrush(90, 90, 90);
        BorderThickness = new Thickness(2);

        Padding = new Thickness(10, 5, 10, 5);

        CornerRadius = new CornerRadius(10);

        MainPanel.Children.Add(_nodeTitle = CreateNodeTitle(title));

        Child = MainPanel;

        Margin = new Thickness(0, 0, 0, 10);
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {
        if (_hoveredNode is not null)
        {
            _hoveredNode.SetIsDirectlyHovered(false);
        }
        SetIsDirectlyHovered(true);
        _hoveredNode = this;
        e.Handled = true;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        SetIsDirectlyHovered(false);

        if (_parentNode is not null)
        {
            _parentNode.SetIsDirectlyHovered(true);
            _hoveredNode = _parentNode;
        }
        e.Handled = true;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (IsNotRootNode && _isDirectlyHovered)
        {
            GraphPanel.CurrentDraggedNode = this;
            IsHitTestVisible = false;
        }
        e.Handled = true;
    }

    public void SetIsDirectlyHovered(bool state) 
    {
        if (IsNotRootNode && state)
        {
            _isDirectlyHovered = true;
            Background = _hoverBackground;
        }
        else
        {
            _isDirectlyHovered = false;
            Background = _normalBackground;
        }
    }

    private static Brush _normalBackground = BrushHelper.MakeSolidBrush(245, 245, 245);
    private static Brush _hoverBackground = BrushHelper.MakeSolidBrush(220, 220, 220);

    protected abstract NodeTitle CreateNodeTitle(string title);
}

public abstract class GraphNodeWithBody : GraphNode
{
    public UIElementCollection Nodes 
    {
        get => _bodyPanel.Children;
    }

    private NodeStackPanel _bodyPanel;

    public GraphNodeWithBody(string title, GraphNode? parent) : base(title, parent)
    {
        _bodyPanel = new NodeStackPanel() 
        { 
            Margin = new Thickness(0, 10, 0, -2)
        };

        _bodyPanel.Children.Add(new CallFunctionNode("Print [String Message](Hello Tyler!) to console", this));
        _bodyPanel.Children.Add(new CallFunctionNode("Print [String Message](Hello Tyler!) to console", this));

        MainPanel.Children.Add(_bodyPanel);
    }
}

public class DefineFunctionNode : GraphNodeWithBody
{
    public override bool IsNotRootNode => false;

    public DefineFunctionNode(string title, GraphNode? parent) : base(title, parent) 
    {

    }

    protected override NodeTitle CreateNodeTitle(string title)
    {
        return new DefineParametersNodeTitle(title);
    }
}

public class CallFunctionNode : GraphNode
{
    public CallFunctionNode(string title, GraphNode? parent) : base(title, parent)
    {

    }

    protected override NodeTitle CreateNodeTitle(string title)
    {
        return new FeedParametersNodeTitle(title);
    }
}

public abstract class NodeTitle : ContentControl
{
    private InlineEditableTextBox _textbox;

    public string Title 
    {
        get => _textbox.Text;
        set => _textbox.Text = value;
    }

    protected NodeTitle(string title, bool isEditable) 
    {
        Content = _textbox = new InlineEditableTextBox(isEditable)
        {
            Text = title
        };
        _textbox.PopulateInlines += PopulateTextBlockInlines;
        _textbox.RefreshTextBlockInlines();
    }

    protected abstract void PopulateTextBlockInlines(string text, InlineCollection inlines);
}

public class DefineParametersNodeTitle : NodeTitle
{
    public DefineParametersNodeTitle(string title) : base(title, true)
    {
        
    }

    protected override void PopulateTextBlockInlines(string text, InlineCollection inlines)
    {
        string trimmedText = text.Trim();

        bool isWithinParameter = false;

        // [Type Name]
        string currentParamterType = string.Empty;
        string currentParamterName = string.Empty;

        bool isReadingParamterName = false;

        Inline currentInline = new Run();

        void AdvanceInline(Inline newInline)
        {
            inlines.Add(currentInline);
            currentInline = newInline;
        }

        for (int i = 0; i < trimmedText.Length; i++)
        {
            char Character = trimmedText[i];

            if (isWithinParameter)
            {
                if (Character == '}')
                {
                    isWithinParameter = false;
                    AdvanceInline(new InlineUIContainer()
                    {
                        Child = new FunctionDeclareParamter(currentParamterName, currentParamterType)
                    });
                    AdvanceInline(new Run());
                    currentParamterType = string.Empty;
                    currentParamterName = string.Empty;
                    isReadingParamterName = false;
                }
                else if (Character == ' ')
                {
                    isReadingParamterName = true;
                }
                else
                {
                    if (isReadingParamterName)
                    {
                        currentParamterName += Character;
                    }
                    else
                    {
                        currentParamterType += Character;
                    }
                }
            }
            else if (Character == '{')
            {
                isWithinParameter = true;
            }
            else
            {
                if (currentInline is Run run)
                {
                    run.Text += Character;
                }
            }
        }

        inlines.Add(currentInline);
    }

    private class FunctionDeclareParamter : StackPanel
    {
        public FunctionDeclareParamter(string name, string type)
        {
            Margin = new Thickness(0, 0, 0, -7);

            Children.Add(new ArgonTextBlock()
            {
                Text = type,
                FontSize = GraphStyle.HeaderFontSize,
                Foreground = GraphStyle.Type,
                FontFamily = GraphStyle.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, -4)
            });

            Children.Add(new ArgonTextBlock()
            {
                Text = name,
                FontSize = 25,
                Foreground = GraphStyle.Parameter,
                HorizontalAlignment = HorizontalAlignment.Center
            });
        }
    }
}

public class FeedParametersNodeTitle : NodeTitle
{
    public FeedParametersNodeTitle(string title) : base(title, false)
    {
        
    }

    protected override void PopulateTextBlockInlines(string text, InlineCollection inlines)
    {
        string trimmedText = text.Trim();
        
        bool isWithinParameter = false;
        
        // [Type Name](Value)
        string currentParamterType = string.Empty;
        string currentParamterName = string.Empty;
        string currentParameterValue = string.Empty;

        bool isReadingParameterValue = false;
        bool isReadingParameterName = false;
        
        Inline currentInline = new Run();
        
        void AdvanceInline(Inline newInline)
        {
            inlines.Add(currentInline);
            currentInline = newInline;
        }

        void FinishReadingParameter() 
        {
            isWithinParameter = false;
            AdvanceInline(new InlineUIContainer()
            {
                Child = new FunctionFeedParamter(currentParamterName, currentParamterType, ArgonTypesManager.CreateCoreTypeFromNameWithDefaultValue(currentParamterType, currentParameterValue))
            });
            AdvanceInline(new Run());
            currentParamterType = string.Empty;
            currentParamterName = string.Empty;
            currentParameterValue = string.Empty;
            isReadingParameterName = false;
        }
        
        for (int i = 0; i < trimmedText.Length; i++)
        {
            char Character = trimmedText[i];
        
            if (isWithinParameter)
            {
                if (isReadingParameterValue)
                {
                    if (Character == ')')
                    {
                        FinishReadingParameter();
                    }
                    else
                    {
                        currentParameterValue += Character;
                    }
                }
                else if (Character == ']')
                {
                    if (trimmedText[i + 1] == '(')
                    {
                        isReadingParameterValue = true;
                        i++;
                    }
                    else
                    {
                        FinishReadingParameter();
                    }
                }
                else if (Character == ' ')
                {
                    isReadingParameterName = true;
                }
                else
                {
                    if (isReadingParameterName)
                    {
                        currentParamterName += Character;
                    }
                    else
                    {
                        currentParamterType += Character;
                    }
                }
            }
            else if (Character == '[')
            {
                isWithinParameter = true;
            }
            else
            {
                if (currentInline is Run run)
                {
                    run.Text += Character;
                }
            }
        }
        
        inlines.Add(currentInline);
    }

    private class FunctionFeedParamter : StackPanel
    {
        public FunctionFeedParamter(string name, string type, object value)
        {
            VerticalAlignment = VerticalAlignment.Top;

            Margin = new Thickness(0, 0, 0, -8);

            Children.Add(ArgonTypesManager.CreateWidgetForType(value));

            ToolTip = $"{type} | {name}";
        }
    }
}

public class IntegerDefaultValue : ArgonTransparentTextBox
{
    public IntegerDefaultValue(int number) 
    {
        FontSize = GraphStyle.NumberFontSize;
        Foreground = GraphStyle.Number;

        Text = number.ToString();
        Margin = new Thickness(0, -8, 0, -8);

        TextChanged += ValidateText;
    }

    private void ValidateText(object sender, TextChangedEventArgs e)
    {
        string localText = Text;

        string resultText = string.Empty;

        for (int i = 0; i < localText.Length; i++)
        {
            if (char.IsDigit(localText[i]))
            {
                resultText += localText[i];
            }
        }

        Text = resultText;
    }
}

public class StringDefaultValue : ArgonTextBlock
{
    public StringDefaultValue(string text) 
    {
        FontSize = 25;
        Foreground = GraphStyle.String;

        Inlines.Add(new Run("\""));

        Inlines.Add(new InlineUIContainer(new ArgonTransparentTextBox() 
        {
            Text = text,
            Margin = new Thickness(0, 0, 0, -5),
            Foreground = GraphStyle.String
        }));

        Inlines.Add(new Run("\""));
    }
}

public class NodeStackPanel : StackPanel
{
    private static Border PreviewWidget = new Border()
    {
        Height = 5,
        Background = BrushHelper.MakeSolidBrush(200, 200, 200),
        Margin = new Thickness(0, -5, 0, 0),
        CornerRadius = new CornerRadius(2.5)
    };

    public NodeStackPanel() 
    {
        Background = Brushes.Transparent;
    }

    protected override void OnMouseEnter(MouseEventArgs e)
    {

    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (PreviewWidget.Parent == this)
        {
            Children.Remove(PreviewWidget);
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        Point mousePosition = Mouse.GetPosition(this);

        if (PreviewWidget.Parent is Panel panel)
        {
            panel.Children.Remove(PreviewWidget);
        }

        for (int i = 0; i < Children.Count; i++)
        {
            UIElement currentChild = Children[i];
        }

        Children.Insert(0, PreviewWidget);
    }
}

public class ArgonTextBlock : TextBlock
{
    public ArgonTextBlock() 
    {
        FontSize = GraphStyle.NormalFontSize;
        FontFamily = GraphStyle.Normal;
    }
}

public class ArgonTextBox : TextBox
{
    public ArgonTextBox() 
    {
        MinWidth = 10;

        FontSize = GraphStyle.NormalFontSize;
        FontFamily = GraphStyle.Normal;
    }
}

public class ArgonTransparentTextBox : ArgonTextBox
{
    public ArgonTransparentTextBox() 
    {
        BorderThickness = new Thickness();
        BorderBrush = null;
        Background = null;
    }
}

public static class ArgonTypesManager 
{
    public static object CreateCoreTypeFromNameWithDefaultValue(string name, string value) 
    {
        bool useDefaultValue = string.IsNullOrWhiteSpace(value);

        if (name == "String")
        {
            return useDefaultValue ? string.Empty : value;
        }
        else if (name == "Integer")
        {
            return useDefaultValue ? 0 : int.Parse(value);
        }

        throw new NotImplementedException($"The type {name} is not implemented.");
    }

    public static FrameworkElement CreateWidgetForType(object value) 
    {
        if (value is string str)
        {
            return new StringDefaultValue(str)
            {
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }
        else if (value is int integer32)
        {
            return new IntegerDefaultValue(integer32)
            {
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }

        throw new NotImplementedException($"The type {value.GetType().Name} is not implemented.");
    }
}

public static class GraphStyle 
{
    public static Brush Parameter { get; } = BrushHelper.MakeSolidBrush(54, 162, 228);
    public static Brush Type { get; } = BrushHelper.MakeSolidBrush(6, 169, 164);
    public static Brush String { get; } = BrushHelper.MakeSolidBrush(255, 103, 45);
    public static Brush Number { get; } = BrushHelper.MakeSolidBrush(129, 214, 46);

    public static FontFamily Normal { get; } = new FontFamily("Candara");
    public static FontFamily Bold { get; } = new FontFamily("Candara Bold");

    public static double NormalFontSize { get; } = 25;
    public static double HeaderFontSize { get; } = 18;
    public static double NumberFontSize { get; } = 40;
}