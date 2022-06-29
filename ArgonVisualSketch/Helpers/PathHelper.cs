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
}