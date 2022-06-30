using System;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using Argon.Helpers;

namespace ArgonVisual.Helpers;

public static class PathHelper 
{
    public const string DefaultFolderName = "NewFolder";
    public const string DefaultCodeFileName = "NewCodeFile";

    public static string MakeUniqueFolderName(string directoryName, string folderName)
    {
        string[] files = Directory.GetDirectories(directoryName);
        uint number = 0;

        string resultName = folderName;

        while (files.Any((item) => (number > 0 ? resultName + number.ToString() : resultName) == item.SubstringAfterLast(Path.DirectorySeparatorChar)))
        {
            number++;
        }

        return number > 0 ? resultName + number.ToString() : resultName;
    }

    public static string MakeUniqueFileName(string directoryName, string fileName)
    {
        string[] files = Directory.GetFiles(directoryName);
        uint number = 0;

        string resultName = fileName;

        while (files.Any((item) => (number > 0 ? resultName + number.ToString() : resultName) == item.SubstringAfterLast(Path.DirectorySeparatorChar).SubstringBefore('.')))
        {
            number++;
        }

        return resultName;
    }

    /// <summary>
    /// Renames a file
    /// </summary>
    /// <param name="beforeFullFilename">The full name of the file including the directory.</param>
    /// <param name="newName">The new name of the file without the directory and with the extension.</param>
    public static void RenameFile(string beforeFullFilename, string newName)
    {
        File.Move(beforeFullFilename, beforeFullFilename.SubstringBeforeWithLast(Path.DirectorySeparatorChar) + newName);
    }

    /// <summary>
    /// Renames a directory
    /// </summary>
    /// <param name="beforeDirectoryName">The full name of the directory.</param>
    /// <param name="newFolderName">The new name of the folder with out the sub directorys</param>
    /// <returns>The full new name of the directory</returns>
    public static string RenameFolder(string beforeDirectoryName, string newFolderName)
    {
        string newFullName = beforeDirectoryName.SubstringBeforeWithLast(Path.DirectorySeparatorChar) + newFolderName;
        Directory.Move(beforeDirectoryName, newFullName);
        return newFullName;
    }
}