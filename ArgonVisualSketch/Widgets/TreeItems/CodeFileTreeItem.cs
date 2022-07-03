using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ArgonVisual.Helpers;
using ArgonVisual.Views;
using ArgonVisual.Widgets;

namespace ArgonVisual.TreeItems;

/// <summary>
/// Represents a code file in a Argon tree item.
/// </summary>
public class CodeFileTreeItem : ArgonTreeItem
{
    /// <summary>
    /// The <see cref="ArgonCodeFile"/> that this represents.
    /// </summary>
    public ArgonCodeFile CodeFile { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="CodeFileTreeItem"/>.
    /// </summary>
    /// <param name="codeFile">The <see cref="ArgonCodeFile"/> that represents this.</param>
    /// <param name="editor">The editor that owns the tree item.</param>
    public CodeFileTreeItem(ArgonCodeFile codeFile, SolutionEditor editor) : base(codeFile.Name, editor)
    {
        CodeFile = codeFile;
    }

    public static CodeFileTreeItem CreateNewCodeFileInDirectory(DirectoryInfo directory, SolutionEditor editor)
    {
        string codeFilePath = Path.Combine(directory.FullName, PathHelper.MakeUniqueFileName(directory.FullName, "NewCodeFile")) + ArgonFileExtensions.CodeFile;
        ArgonCodeFile newCodeFile = ArgonCodeFile.Create(new FileInfo(codeFilePath));
        CodeFileTreeItem newCodeFileItem = new CodeFileTreeItem(newCodeFile, editor);
        newCodeFileItem.RenameItem();
        newCodeFileItem.IsSelected = true;
        return newCodeFileItem;
    }

    protected override string? IsValidName(string name)
    {
        for (int i = 0; i < name.Length; i++)
        {
            if (!char.IsLetter(name[i]))
            {
                return "File name can only contain letters";
            }
        }

        if (CodeFile.FileInfo.Directory is not null 
         && CodeFile.FileInfo.Directory.EnumerateFiles($"*{ArgonFileExtensions.CodeFile}").Any((info) => info.Name.Equals(name, StringComparison.Ordinal)))
        {
            return $"A file named \"{name}\" already exists.";
        }

        return null;
    }

    protected override void RenameItemInternal(string newName)
    {
        CodeFile.Rename(newName);
    }

    protected override ImageSource GetIcon()
    {
        return ArgonStyle.Icons.CodeFile;
    }

    protected override void OnSelected(RoutedEventArgs e)
    {
        DocumentEditorView? documentView = Editor.FindView<DocumentEditorView>();
        if (documentView is not null)
        {
            documentView.ShowCodeFile(CodeFile);
        }
    }
}