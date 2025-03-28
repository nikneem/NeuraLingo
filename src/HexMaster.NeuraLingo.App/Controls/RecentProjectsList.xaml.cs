using System.ComponentModel;
using System.Text.Json;
using HexMaster.NeuraLingo.App.Models;

namespace HexMaster.NeuraLingo.App.Controls;

public partial class RecentProjectsList
{

	private List<RecentProject> _recentProjects;

	public RecentProjectsList()
	{
		InitializeComponent();
		_recentProjects = [];
        LoadRecentProjectsFromSettings();
	}

    
	private void LoadRecentProjectsFromSettings()
	{
		if (Preferences.ContainsKey("nl-recent-projects"))
		{
			var recentProjectsJson = Preferences.Get("nl-recent-projects", "[]");
			var recentProjects = JsonSerializer.Deserialize<List<RecentProject>>(recentProjectsJson);
			_recentProjects = recentProjects ?? [];
            MainPageRecentProjects.Children.Clear();

            // Add a RecentProjectListItem for each project
            foreach (var project in _recentProjects)
            {
                var projectItem = new RecentProjectListItem
                {
                    BindingContext = project // Bind the project data to the item
                };
                MainPageRecentProjects.Children.Add(projectItem);
            }
		}

	}

    private void CreateNewProject()
    {
        
    }
}