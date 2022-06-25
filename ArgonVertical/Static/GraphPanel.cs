using System;
using System.Net;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using ArgonUserInterfacePrototype;

namespace ArgonVertical.Static;

public class GraphPanel : Border
{
    private static GraphPanel? _global;

    public static GraphNode? CurrentDraggedNode;

    private StackPanel _functionsCategoriesPanel;
    private StackPanel _propertiesPanel;

    private ScrollViewer _functionsScrollViewer;

    public bool IsPanning { get; private set; }

    private double _verticalStartOffset;
    private double _horizontalStartOffset;

    private Point _mouseStartOffset;

    private const double PanningSpeed = 1.5;

    public static GraphPanel Global 
    { 
        get => _global ?? throw new NullReferenceException("GraphPanel has not been instanced!");
        set => _global = value; 
    }

    public GraphPanel()
    {
        _global = this;

        _functionsCategoriesPanel = new StackPanel() 
        {
            Margin = new Thickness(30)
        };
        _propertiesPanel = new StackPanel();
        Background = BrushHelper.MakeSolidBrush(210, 210, 210);

        Grid classesAndEditorPanel = new Grid();

        Grid propertiesAndFunctions = new Grid();

        FunctionCategory category = new FunctionCategory("The personal life category");
        category.AddFunction(new DefineFunctionNode("When the program starts:", null));
        category.AddFunction(new DefineFunctionNode("Do some low-level stuff", null));
        category.AddFunction(new DefineFunctionNode("Give tyler a pep-talk", null));
        category.AddFunction(new DefineFunctionNode("Play with oscar and rosie", null));

        FunctionCategory otherCategory = new FunctionCategory("The programming world category");
        otherCategory.AddFunction(new DefineFunctionNode("Create Editor", null));
        otherCategory.AddFunction(new DefineFunctionNode("Compile Code", null));
        otherCategory.AddFunction(new DefineFunctionNode("Start game in new window", null));
        otherCategory.AddFunction(new DefineFunctionNode("Get tyler to react to your video", null));

        _functionsCategoriesPanel.Children.Add(category);
        _functionsCategoriesPanel.Children.Add(otherCategory);

        propertiesAndFunctions.AddColumnAuto(new Border() 
        {
            Background = BrushHelper.MakeSolidBrush(190, 190, 190),
            Child = new ScrollViewer() 
            {
                Content = _propertiesPanel,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
            }
        });

        for (int i = 0; i < 40; i++)
        {
            _propertiesPanel.Children.Add(new PropertyWidget());
        }

        propertiesAndFunctions.AddColumnFill(_functionsScrollViewer = new ScrollViewer()
        {
            HorizontalScrollBarVisibility = ScrollBarVisibility.Visible,
            VerticalScrollBarVisibility = ScrollBarVisibility.Visible,
            PanningMode = PanningMode.Both,
            Content = _functionsCategoriesPanel
        });

        classesAndEditorPanel.AddRowPixel(60, new ClassesViewer());

        classesAndEditorPanel.AddRowFill(propertiesAndFunctions);

        Child = classesAndEditorPanel;
    }

    protected override void OnPreviewMouseMove(MouseEventArgs e)
    {
        if (IsPanning)
        {
            Point mousePosition = e.GetPosition(this);
            _functionsScrollViewer.ScrollToHorizontalOffset(((mousePosition.X * -1) + _mouseStartOffset.X) * PanningSpeed + _horizontalStartOffset);
            _functionsScrollViewer.ScrollToVerticalOffset(((mousePosition.Y * -1) + _mouseStartOffset.Y) * PanningSpeed + _verticalStartOffset);
        }
    }

    protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Right)
        {
            _mouseStartOffset = e.GetPosition(this);
            _verticalStartOffset = _functionsScrollViewer.VerticalOffset;
            _horizontalStartOffset = _functionsScrollViewer.HorizontalOffset;
            IsPanning = true;
        }
    }

    protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
    {
        IsPanning = false;
    }

    private void CreateNewFunction(object sender, RoutedEventArgs e)
    {
        GraphNode newNode = new DefineFunctionNode("My Function", null);
        _functionsCategoriesPanel.Children.Add(newNode);
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (CurrentDraggedNode is not null)
        {
            CurrentDraggedNode.StopDragging();
        }
        CurrentDraggedNode = null;
    }
}

public class ClassesViewer : Border
{
    public ClassesViewer()
    {
        Background = BrushHelper.MakeSolidBrush(200, 200, 200);

        Child = new ArgonTextBlock()
        {
            Text = "Classes Viewer",
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center
        };
    }
}

public class FunctionCategory : Grid
{
    private StackPanel _functionRootNodes;

    public FunctionCategory(string catgoryName) 
    {
        Margin = new Thickness(0, 0, 0, 20);

        _functionRootNodes = new StackPanel() 
        {
            Orientation = Orientation.Horizontal
        };

        this.AddRowAuto(new Border() 
        {
            Background = BrushHelper.MakeSolidBrush(255, 126, 236),
            Child = new ArgonTextBlock() 
            {
                Text = catgoryName
            }
        });

        this.AddRowAuto(new Border() 
        {
            Background = BrushHelper.MakeSolidBrush(255, 201, 247),
            Padding = new Thickness(0, 20, 0, 5),
            Child = _functionRootNodes
        });
    }

    public void AddFunction(DefineFunctionNode functionNode) 
    {
        _functionRootNodes.Children.Add(functionNode);
    }
}

public class PropertyWidget : Border
{
    public PropertyWidget() 
    {
        Background = BrushHelper.MakeSolidBrush(230, 230, 230);

        Margin = new Thickness(10);

        HorizontalAlignment = HorizontalAlignment.Left;

        StackPanel stackPanel = new StackPanel() 
        {
            Margin = new Thickness(8, 3, 8, 3)
        };

        stackPanel.Children.Add(new ArgonTextBlock()
        {
            Text = "PropertyType",
            FontSize = GraphStyle.HeaderFontSize + 2
        });

        stackPanel.Children.Add(new ArgonTextBlock() 
        {
            Text = "SkeletalMeshBuildSettings",
            Margin = new Thickness(0, -5, 0, 0),
            FontSize = GraphStyle.NormalFontSize + 5
        });

        Child = stackPanel;
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
        VerticalAlignment = VerticalAlignment.Top;

        BorderBrush = GraphStyle.NormalBorder;
        BorderThickness = new Thickness(2);

        Padding = new Thickness(10, 5, 10, 5);
        CornerRadius = new CornerRadius(10);
        Margin = new Thickness(0, 0, 0, 10);

        MainPanel.Children.Add(_nodeTitle = CreateNodeTitle(title));
        Child = MainPanel;
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
        if (GraphPanel.CurrentDraggedNode != this)
        {
            SetIsDirectlyHovered(false);

            if (_parentNode is not null)
            {
                _parentNode.SetIsDirectlyHovered(true);
                _hoveredNode = _parentNode;
            }
        }

        e.Handled = true;
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        if (IsNotRootNode && _isDirectlyHovered)
        {
            StartDragging();
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

    public void StartDragging() 
    {
        IsHitTestVisible = false;
        GraphPanel.CurrentDraggedNode = this;
    }

    public void StopDragging() 
    {
        IsHitTestVisible = true;
    }

    private static Brush _normalBackground = BrushHelper.MakeSolidBrush(245, 245, 245);
    private static Brush _hoverBackground = BrushHelper.MakeSolidBrush(225, 225, 225);

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

        _bodyPanel.Children.Add(new CallFunctionNode("Print [String Message](Oscar is the best dog!) to console", this));
        _bodyPanel.Children.Add(new CallFunctionNode("Print [String Message](Tyler is the coolest person ever!) to console", this));
        _bodyPanel.Children.Add(new CallFunctionNode("Print [String Message](Tyler is the coolest person ever!) to console", this));
        _bodyPanel.Children.Add(new CallFunctionNode("Print [String Message](Tyler is the coolest person ever!) to console", this));
        _bodyPanel.Children.Add(new CallFunctionNode("Print [String Message](Tyler is the coolest person ever!) to console", this));
        _bodyPanel.Children.Add(new CallFunctionNode("Print [String Message](Tyler is the coolest person ever!) to console", this));
        _bodyPanel.Children.Add(new CallFunctionNode("Print [String Message](Tyler is the coolest person ever!) to console", this));

        MainPanel.Children.Add(_bodyPanel);
    }
}

public class DefineFunctionNode : GraphNodeWithBody
{
    public override bool IsNotRootNode => false;

    public DefineFunctionNode(string title, GraphNode? parent) : base(title, parent) 
    {
        Margin = new Thickness(30, 0, 30, 10);
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
        HorizontalAlignment = HorizontalAlignment.Center;
        VerticalAlignment = VerticalAlignment.Top;

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
        CornerRadius = new CornerRadius(2.5),
        RenderTransform = new TranslateTransform(0, -2.5)
    };

    public NodeStackPanel() 
    {
        Background = Brushes.Transparent;
    }

    protected override void OnMouseLeave(MouseEventArgs e)
    {
        if (PreviewWidget.Parent == this)
        {
            Children.Remove(PreviewWidget);
        }
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left && GraphPanel.CurrentDraggedNode is not null)
        {
            if (GraphPanel.CurrentDraggedNode.Parent is NodeStackPanel nodeStackPanel)
            {
                nodeStackPanel.Children.Remove(GraphPanel.CurrentDraggedNode);
            }
            
            int insertIndex = CaclulateInsertIndexFromMousePosition(Children.IndexOf(GraphPanel.CurrentDraggedNode));
            if (insertIndex != -1)
            {
                Children.Insert(insertIndex, GraphPanel.CurrentDraggedNode);
            }

            GraphPanel.CurrentDraggedNode.StopDragging();
            GraphPanel.CurrentDraggedNode = null;
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        if (GraphPanel.CurrentDraggedNode is not null)
        {
            int insertIndex = CaclulateInsertIndexFromMousePosition(Children.IndexOf(GraphPanel.CurrentDraggedNode));
            if (insertIndex != -1)
            {
                Children.Insert(insertIndex, PreviewWidget);
            }
        }
    }

    private int CaclulateInsertIndexFromMousePosition(int nodeIndex = -1) 
    {
        bool hasValidNodeIndex = nodeIndex != -1;

        Point mousePosition = Mouse.GetPosition(this);

        if (PreviewWidget.Parent is Panel panel)
        {
            panel.Children.Remove(PreviewWidget);
        }

        double currentYOffset = 0;
        double lastWidgetHalfHeight = 1;
        int index = 0;

        for (int i = 0; i < Children.Count; i++)
        {
            UIElement currentChild = Children[i];
            currentYOffset += currentChild.RenderSize.Height + 10;
            lastWidgetHalfHeight = (currentChild.RenderSize.Height + 10) / 2;

            if (currentYOffset - lastWidgetHalfHeight > mousePosition.Y)
            {
                break;
            }

            index++;
        }

        int confinedIndex = index;

        int validIndex = confinedIndex < 0 || confinedIndex > Children.Count ? -1 : confinedIndex;

        return validIndex;
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

    public static Brush NormalBorder { get; } = BrushHelper.MakeSolidBrush(90, 90, 90);
    public static Brush HoverBorder { get; } = BrushHelper.MakeSolidBrush(33, 161, 218);

    public static FontFamily Normal { get; } = new FontFamily("Candara");
    public static FontFamily Bold { get; } = new FontFamily("Candara Bold");

    public static double NormalFontSize { get; } = 22;
    public static double HeaderFontSize { get; } = 15;
    public static double NumberFontSize { get; } = 40;
}