namespace HexMaster.NeuraLingo.Core.Project.DomainModels
{
    public class TranslationValue
    {
        public TranslationValue(string languageId, string value)
        {
            LanguageId = languageId;
            Value = value;
        }

        public string LanguageId { get; set; }
        public string Value { get; set; }
        public bool IsDefault { get; set; }
        public bool IsGenerated { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsCustomized { get; set; }
    }
}