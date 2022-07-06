using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ArgonVisual.Widgets;

public class FunctionNameTextBlock : ContentControl
{
    private string _text;

    TextBlock _nameText;

    public new string Text 
    {
        get => _text;
        set 
        {
            _text = value;
            RefreshText(value);
        }
    }

    public FunctionNameTextBlock() 
    {
        _text = string.Empty;
        _nameText = new TextBlock();
        VerticalAlignment = VerticalAlignment.Center;

        Content = _nameText;
    }

    private void RefreshText(string newText) 
    {
        _nameText.Inlines.Clear();
        PopulateInlinesFromText(newText, _nameText.Inlines);
    }

    public static void PopulateInlinesFromText(string text, InlineCollection inlines) 
    {
        bool isReadingParameter = false;

        StringBuilder builder = new StringBuilder();

        inlines.Clear();

        for (int i = 0; i < text.Length; i++)
        {
            char character = text[i];
            if (character == '{' && !isReadingParameter)
            {
                if (builder.Length > 0)
                {
                    inlines.Add(new Run(builder.ToString())
                    {
                        BaselineAlignment = BaselineAlignment.Center
                    });
                    builder.Clear();
                }
                isReadingParameter = true;
            }
            else if (isReadingParameter && character == '}')
            {
                inlines.Add(new InlineUIContainer(new FunctionInlineParameter(builder.ToString()))
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

        inlines.Add(new Run(builder.ToString())
        {
            BaselineAlignment = BaselineAlignment.Center
        });
        builder.Clear();
    }

    private class FunctionInlineParameter : Border
    {
        public string Text 
        {
            get => _nameText.Text;
            set => _nameText.Text = value;
        }

        private TextBlock _nameText;

        public FunctionInlineParameter(string text) 
        {
            Background = BrushHelper.MakeSolidBrush(39, 114, 145);

            CornerRadius = new CornerRadius(5);

            _nameText = new TextBlock()
            {
                Text = text,
                Margin = new Thickness(5, 0, 5, 0)
            };

            Child = _nameText;
        }
    }
}