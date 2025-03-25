using System.IO.Compression;
using System.Text.Json;

namespace HexMaster.NeuraLingo.Core.Project.DomainModels;

public class Project
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ProjectFilename { get; set; }
    public List<string> SupportedLanguages { get; set; } = [];
    public List<TranslationKey> Translations { get; set; } = [];
    public List<ProjectOutputFile> OutputFiles { get; set; } = [];
    public string? OutputPath { get; set; }
    public required string DefaultLanguage { get; set; }
    public required string SourceFile { get; set; }
    public required string Version { get; set; }

    #region [ Import from i18n resource file ]

    public async Task<List<TranslationKey>> ImportFromFile(
        Stream stream,
        string defaultLanguageId,
         CancellationToken cancellationToken)
    {
        var reader = new StreamReader(stream);
        string jsonContent = await reader.ReadToEndAsync(cancellationToken);
        var translations = ParseJson(jsonContent, defaultLanguageId);
        return translations;
    }

    private List<TranslationKey> ParseJson(string jsonContent, string defaultLanguageId)
    {
        var translations = new List<TranslationKey>();
        var translationsAlternative = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        var parentKey = string.Empty;

        foreach (var translationRootNode in translationsAlternative)
        {
            var translationNode = ParseNode(translationRootNode.Key, parentKey, defaultLanguageId, translationRootNode.Value);
            translations.Add(
                new TranslationKey(
                    translationNode.Key,
                    translationNode.FullKeyPath,
                    translationNode.Children,
                    translationNode.TranslationValues));
        }

        return translations;
    }

    private List<TranslationKey> GetChildNodes(string parentKey, string languageId, JsonElement childNodes)
    {
        var deserializedChildNodes = JsonSerializer.Deserialize<Dictionary<string, object>>(
            childNodes.ToString(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        var nodes = new List<TranslationKey>();
        foreach (var kvp in deserializedChildNodes)
        {
            var translationNode = ParseNode(kvp.Key, parentKey, languageId, kvp.Value);
            nodes.Add(translationNode);
        }

        return nodes;
    }


    private TranslationKey ParseNode(string key, string? parentKey, string languageId, object childNode)
    {
        parentKey = string.IsNullOrWhiteSpace(parentKey) ? key : $"{parentKey}.{key}";
        var values = new List<TranslationValue>();
        List<TranslationKey>? children = null;
        if (childNode is JsonElement jsonElement)
        {
            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                var value = jsonElement.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    values.Add(new TranslationValue(languageId, value));
                }
            }

            if (jsonElement.ValueKind == JsonValueKind.Object)
            {
                children = GetChildNodes(parentKey, languageId, jsonElement);
            }
        }

        return new TranslationKey(key, parentKey, children, values);
    }



    #endregion

    #region [ Export to i18n resource file ]

    public async Task ExportToFile(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(OutputPath))
        {
            throw new InvalidOperationException("The output path is not set");
        }

        foreach (var outputFile in OutputFiles)
        {
            var outputFilePath = Path.Combine(OutputPath, outputFile.Filename);
            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }

            await using var fileStream = new FileStream(outputFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            var jsonWriterOptions = new JsonWriterOptions { Indented = true };
            var writer = new Utf8JsonWriter(fileStream, jsonWriterOptions);
            writer.WriteStartObject();
            foreach (var node in Translations)
            {
                WriteNode(writer, node, outputFile.LanguageId, DefaultLanguage);
            }
            writer.WriteEndObject();
            await writer.FlushAsync(cancellationToken);
        }
    }

    private void WriteNode(Utf8JsonWriter writer, TranslationKey node, string language, string defaultLanguage)
    {
        if (node.Children?.Count > 0)
        {
            writer.WriteStartObject(node.Key);
            foreach (var child in node.Children)
            {
                WriteNode(writer, child, language, defaultLanguage);
            }
            writer.WriteEndObject();
        }
        else
        {
            var value = node.GetValueForLanguage(language);
            writer.WriteString(node.Key, value);
        }
    }

    #endregion

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