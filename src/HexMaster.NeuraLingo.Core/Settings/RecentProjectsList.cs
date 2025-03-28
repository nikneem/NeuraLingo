using System.Text.Json;
using Microsoft.Maui.Storage;

namespace HexMaster.NeuraLingo.Core.Settings;

public class RecentProjectsList
{
    private const string PreferencesKey = "nl-recent-projects";
    public Lazy<List<RecentProject>> RecentProjects => new(() => Load());

    public void Opened(string name, string filename)
    {
        var updatedList = RecentProjects.Value;
        var existingIndex = updatedList.FindIndex(p => p.Filename == filename);
        updatedList.RemoveAt(existingIndex);

        var existingProject = new RecentProject(
             name,
             filename,
            DateTimeOffset.UtcNow
        );
        updatedList.Add(existingProject);

        if (updatedList.Count > 10)
        {
            var oldestRecentProject = updatedList.OrderBy(rp => rp.LastWorkedOn).First();
            updatedList.Remove(oldestRecentProject);
        }

        Save(updatedList);
    }
    public void Save(List<RecentProject>? projects = null)
    {
        var recentProjectsList = projects ?? RecentProjects.Value;
        var json = JsonSerializer.Serialize(recentProjectsList);
        Preferences.Set(PreferencesKey, json);
    }
    public static List<RecentProject> Load()
    {
        if (Preferences.ContainsKey(PreferencesKey))
        {
            var json = Preferences.Get(PreferencesKey, "[]");
            return JsonSerializer.Deserialize<List<RecentProject>>(json) ?? new List<RecentProject>();
        }
        return new List<RecentProject>();
    }
}