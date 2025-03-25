using HexMaster.DomainDrivenDesign;

namespace HexMaster.NeuraLingo.Core.Project.DomainModels;

public class ProjectOutputFile : DomainModel<Guid>
{
    public string LanguageId { get; set; }
    public string Filename { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }

    public ProjectOutputFile(Guid id,
    string languageId,
     string filename,
      DateTimeOffset createdOn,
       DateTimeOffset updatedOn) : base(id)
    {
        LanguageId = languageId;
        Filename = filename;
        CreatedOn = createdOn;
        UpdatedOn = updatedOn;
    }

    private ProjectOutputFile(string languageId, string filename) : base(Guid.NewGuid())
    {
        LanguageId = languageId;
        Filename = filename;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }
    public static ProjectOutputFile Create(string languageId, string filename)
    {
        return new ProjectOutputFile(languageId, filename);
    }


}