using HexMaster.NeuraLingo.Core.Project.DomainModels;
using HexMaster.NeuraLingo.Core.Settings;

namespace HexMaster.NeuraLingo.App.Controls;

public partial class RecentProjectsList
{

	private List<RecentProject> _recentProjects;
	private RecentProjectsService _recentProjectsService;

	public RecentProjectsList()
	{
		InitializeComponent();
		_recentProjects = [];
		_recentProjectsService = new RecentProjectsService();
		LoadRecentProjectsFromSettings();
	}


	private void LoadRecentProjectsFromSettings()
	{
		_recentProjects = _recentProjectsService.RecentProjects.Value;

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

	private Project CreateNewProject()
	{
		// Display new project dialog and return a new project
		// or something, not sure how this should work yet
		var project = new Project("Bananas", "d:\\temp\\bananas.json", "en");
		_recentProjectsService.Opened(project.Name, project.SourceFile);
		return project;
	}
}