namespace HexMaster.NeuraLingo.Core.Project.DomainModels
{
    public class TranslationKey
    {
        public string Key { get; set; }
        public string FullKeyPath { get; set; }
        public List<TranslationValue>? TranslationValues { get; set; } = [];
        public List<TranslationKey>? Children { get; set; } = [];

        public string GetValueForLanguage(string languageId)
        {
            if (TranslationValues == null || TranslationValues.Count == 0)
            {
                return FullKeyPath;
            }
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

        internal TranslationKey(
            string key,
            string fullKeyPath,
            List<TranslationKey>? children,
            List<TranslationValue>? translationValues)
        {
            Key = key;
            FullKeyPath = fullKeyPath;
            Children = children;
            TranslationValues = translationValues;
        }
    }
}