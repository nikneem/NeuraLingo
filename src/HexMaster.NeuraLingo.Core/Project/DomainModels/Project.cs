using System.IO.Compression;
using System.Text.Json;

namespace HexMaster.NeuraLingo.Core.Project.DomainModels;

public class Project
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ProjectFilename { get; set; }
    public List<string> SupportedLanguages { get; set; } = [];
    public List<ProjectOutputFile> OutputFiles { get; set; } = [];
    public required string DefaultLanguage { get; set; }
    public required string SourceFile { get; set; }
    public required string Version { get; set; }


    #region [ New Project Constructor ]

    public Project(string name, string sourceFile, string defaultLanguage)
    {
        Name = name;
        SourceFile = sourceFile;
        DefaultLanguage = defaultLanguage;
        SupportedLanguages.Add(defaultLanguage);
        Version = "1.0";
    }

    #endregion

    #region [ Save / Load project from file ]

    public void SaveToCompressedJson()
    {
        if (string.IsNullOrEmpty(ProjectFilename))
        {
            throw new InvalidOperationException("ProjectFilename is not set");
        }
        using FileStream fs = new(ProjectFilename, FileMode.Create);
        using GZipStream gzip = new(fs, CompressionMode.Compress);
        JsonSerializer.Serialize(gzip, this);
    }
    public static Project LoadFromCompressedJson<T>(string filePath)
    {
        using FileStream fs = new(filePath, FileMode.Open);
        using GZipStream gzip = new(fs, CompressionMode.Decompress);
        var project = JsonSerializer.Deserialize<Project>(gzip);
        if (project == null)
        {
            throw new InvalidOperationException("Project could not be loaded");
        }
        return project;
    }

    #endregion

}