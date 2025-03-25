namespace HexMaster.NeuraLingo.Core.Project.DomainModels
{
    public class TranslationKey
    {
        public required string Key { get; set; }
        public required string FullKeyPath { get; set; }
        public List<TranslationValue> TranslationValues { get; set; } = [];

        public string GetValueForLanguage(string languageId)
        {
            var translationValue = TranslationValues.FirstOrDefault(x => x.LanguageId == languageId)?.Value;
            if (translationValue == null)
            {
                translationValue = TranslationValues.FirstOrDefault(x => x.IsDefault)?.Value;
            }
            if (translationValue == null)
            {
                translationValue = FullKeyPath;
            }
            return translationValue;
        }

    }
}