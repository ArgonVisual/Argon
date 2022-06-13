using System;
using System.IO;

namespace Argon.Helpers;

public static class PathHelper 
{
    /// <summary>
    /// Gets the directory and name (with out extension) from <paramref name="str"/>.
    /// </summary>
    /// <param name="str">The string to get the directory and name from.</param>
    /// <param name="directory">The directory</param>
    /// <param name="name">The name (without extension)</param>
    public static (string directory, string name) GetDirectoryAndName(this string str) 
    {
        string directory = str.SubstringBeforeLast(Path.DirectorySeparatorChar);
        string name = Path.GetFileName(str).SubstringBeforeLast('.');
        return (directory, name);
    }

    /// <summary>
    /// Renames a directory on disk.
    /// </summary>
    /// <param name="directory">The directory to rename.</param>
    /// <exception cref="ArgumentNullException">The directory </exception>
    /// <exception cref="ArgumentException"><paramref name="newName"/> is empty or whitespace.</exception>
    public static void RenameDirectory(this DirectoryInfo directory, string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("New name cannot be null or blank", "name");
        }

        if (directory.Name == newName)
        {
            return;
        }

        if (directory.Parent is not null)
        {
            directory.MoveTo(Path.Combine(directory.Parent.FullName, newName));
        }

        return; //done
    }

    /// <summary>
    /// Renames a file on disk.
    /// </summary>
    /// <param name="file">The file to rename.</param>
    /// <param name="newName">The new name for the file.</param>
    public static void RenameFile(FileInfo file, string newName) 
    {
        if (file.Name == newName)
        {
            return;
        }

        file.MoveTo(Path.Combine(file.FullName.SubstringBeforeLast(Path.DirectorySeparatorChar), newName));
    }
}