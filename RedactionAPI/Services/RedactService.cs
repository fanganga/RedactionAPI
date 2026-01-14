namespace RedactionAPI.Services
{
    public class RedactService(IConfiguration config) : IRedactService 
    {
        private readonly IConfiguration _config = config;
        private readonly char[] _defaultPunctuation = [',', '.', ':', ';', '(', ')', '"', '\''];
        
        public string Redact(string message)
        {
            string? bannedWordsCSV = _config.GetSection("RedactionSettings").GetValue<string>("BannedWords");
            string? punctuationString = _config.GetSection("RedactionSettings").GetValue<string>("Punctuation");

            char[] punctuation;
            if (punctuationString != null && punctuationString != string.Empty)
            {
                punctuation = punctuationString.ToCharArray();
            }
            else
            {
                punctuation = _defaultPunctuation;
            }

            if (bannedWordsCSV != null && bannedWordsCSV != string.Empty)
            {
                IEnumerable<string> bannedWords = bannedWordsCSV.Split(',').ToList().Select(word => word.ToUpper());
                string[] messageWords = message.Split(" ");


                var redactedWords = messageWords.ToList()
                    .Select(word => IsBanned(word, punctuation, bannedWords) ?
                    RedactPreservingPunctuation(word, punctuation, "REDACTED") : word);
                return string.Join(" ", [.. redactedWords]);
            }

            return message;
        }

        private static bool IsBanned(string word, char[] punctuation, IEnumerable<string> bannedWords) 
        {
            string cleanedWord = word.Trim(punctuation).ToUpper();
            return bannedWords.Contains(cleanedWord);
        }

        private static string RedactPreservingPunctuation(string word, char[] punctuation, string redactionMark)
        {
            int leadingPunctuationLength = word.Length - word.TrimStart(punctuation).Length;
            int trailingPunctuationLength = word.Length - word.TrimEnd(punctuation).Length;
            
            string leadingPunctuation = word[..leadingPunctuationLength];
            string trailingPunctuation = word[^trailingPunctuationLength..];

            return leadingPunctuation + redactionMark + trailingPunctuation;
        }


    }
}
