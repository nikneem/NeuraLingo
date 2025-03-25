namespace HexMaster.NeuraLingo.Core.Project.DomainModels;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> SupportedLanguages { get; set; }
    public string DefaultLanguage { get; set; }
}