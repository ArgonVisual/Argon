using System;
using System.Windows.Documents;

namespace ArgonVisual.Widgets;

public class FunctionNameEditableText : EditableText 
{
    private string _internalText;

    public override string Text 
    {
        get => _internalText;
        set
        {
            base.Text = _internalText = value;
            CreateInlinesFromText(value, TextBlock.Inlines);
        }
    }

    public FunctionNameEditableText() : base(CreateInlinesFromText)
    {
        _internalText = string.Empty;
    }

    private static void CreateInlinesFromText(string text, InlineCollection inlines)
    {
        FunctionNameTextBlock.PopulateInlinesFromText(text, inlines);
    }
}