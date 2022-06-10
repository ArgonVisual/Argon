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
}