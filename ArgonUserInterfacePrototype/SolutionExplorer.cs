using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArgonUserInterfacePrototype;

public class SolutionExplorer : ContentControl
{
    public SolutionExplorer() 
    {
        ResourceDictionary resourceDictionary = new ResourceDictionary();
        Style textBlockStyle = new Style(typeof(TextBlock));
        textBlockStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, (double)15));
        resourceDictionary.Add(typeof(TextBlock), textBlockStyle);

        Resources = resourceDictionary;

        TreeView treeView = new TreeView();

        SolutionItem solution = new SolutionItem("MyAmazingSolution");
        {
            SolutionFolderItem solutionFolder = new SolutionFolderItem("SolutionFolder");
            {
                ProjectItem project = new ProjectItem("MyProject");
                {
                    ProjectFolderItem food = new ProjectFolderItem("Food");
                    {
                        food.Items.Add(new CodeFileItem("Apple"));
                        food.Items.Add(new CodeFileItem("Strawberry"));
                        food.Items.Add(new CodeFileItem("Orange"));
                        food.Items.Add(new CodeFileItem("Banana"));
                    }
                    project.Items.Add(food);

                    ProjectFolderItem places = new ProjectFolderItem("Places");
                    {
                        places.Items.Add(new CodeFileItem("School"));
                        places.Items.Add(new CodeFileItem("Post Office"));
                        places.Items.Add(new CodeFileItem("Library"));
                    }
                    project.Items.Add(places);

                    ProjectFolderItem planets = new ProjectFolderItem("Planets");
                    {
                        planets.Items.Add(new CodeFileItem("Earth"));
                        planets.Items.Add(new CodeFileItem("Saturn"));
                        planets.Items.Add(new CodeFileItem("Mars"));
                        planets.Items.Add(new CodeFileItem("Jupiter"));
                    }
                    project.Items.Add(planets);

                    project.Items.Add(new CodeFileItem("Human"));
                }
                solutionFolder.Items.Add(project);
            }
            solution.Items.Add(solutionFolder);
        }
        treeView.Items.Add(solution);
        
        Content = treeView;
    }
}