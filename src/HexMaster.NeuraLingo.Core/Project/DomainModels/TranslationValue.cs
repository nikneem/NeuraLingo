namespace HexMaster.NeuraLingo.Core.Project.DomainModels
{
    public class TranslationValue
    {
        public required string LanguageId { get; set; }
        public required string Value { get; set; }
        public bool IsDefault { get; set; }
        public bool IsGenerated { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCustomized { get; set; }
    }
}