namespace RedactionAPI.Services
{
    public class RedactService(IConfiguration config) : IRedactService 
    {
        private readonly IConfiguration _config = config;
        private readonly char[] _defaultPunctuation = { ',', '.', ':', ';', '(', ')','"','\'' };
        
        public string Redact(string message)
        {
            string? bannedWordsCSV = _config.GetSection("RedactionSettings").GetValue<string>("BannedWords");
            string? punctuationString = _config.GetSection("RedactionSettings").GetValue<string>("Punctuation");

            char[] punctuation;
            if (punctuationString != null && punctuationString != String.Empty)
            {
                punctuation = punctuationString.ToCharArray();
            }
            else
            {
                punctuation = _defaultPunctuation;
            }

            if (bannedWordsCSV != null && bannedWordsCSV != String.Empty)
            {
                IEnumerable<string> bannedWords = bannedWordsCSV.Split(',').ToList().Select(word => word.ToUpper());
                string[] messageWords = message.Split(" ");


                var redactedWords = messageWords.ToList()
                    .Select(word => IsBanned(word, punctuation, bannedWords) ?
                    RedactPreservingPunctuation(word, punctuation, "REDACTED") : word);
                return String.Join(" ", redactedWords.ToArray());
            }

            return message;
        }

        private bool IsBanned(string word, Char[] punctuation, IEnumerable<string> bannedWords) 
        {
            string cleanedWord = word.Trim(punctuation).ToUpper();
            return bannedWords.Contains(cleanedWord);
        }

        private string RedactPreservingPunctuation(string word, Char[] punctuation, string redactionMark)
        {
            int leadingPunctuationLength = word.Length - word.TrimStart(punctuation).Length;
            int trailingPunctuationLength = word.Length - word.TrimEnd(punctuation).Length;
            
            string leadingPunctuation = word.Substring(0, leadingPunctuationLength);
            string trailingPunctuation = word.Substring(word.Length - trailingPunctuationLength);

            return leadingPunctuation + redactionMark + trailingPunctuation;
        }


    }
}
