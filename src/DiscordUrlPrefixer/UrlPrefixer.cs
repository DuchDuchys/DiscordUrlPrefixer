namespace DiscordUrlPrefixer;

public static class UrlPrefixer
{
    private static readonly Dictionary<string, string> DomainMappings = new(StringComparer.OrdinalIgnoreCase)
    {
        { "instagram.com", "kkinstagram.com" },
        { "www.instagram.com", "www.kkinstagram.com" },
        { "twitter.com", "girlcockx.com" },
        { "www.twitter.com", "www.girlcockx.com" },
        { "x.com", "girlcockx.com" },
        { "www.x.com", "www.girlcockx.com" },
    };

    public static (string TransformedMessage, bool HadMatches) ReplaceUrls(string message)
    {
        var transformed = message.Split(' ').Select(TransformWord).ToList();
        var hadMatches = transformed.Any(t => t.Changed);
        return (string.Join(" ", transformed.Select(t => t.Transformed)), hadMatches);
    }

    private static (string Transformed, bool Changed) TransformWord(string word)
    {
        if (word.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            word.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            var result = TransformUrl(word);
            return (Transformed: result, Changed: result != word);
        }
        return (Transformed: word, Changed: false);
    }

   public static string TransformUrl(string url)
{
    var uri = new Uri(url);
    if (DomainMappings.TryGetValue(uri.Host, out var mappedDomain))
    {
        var baseUrl = uri.GetLeftPart(UriPartial.Path);
        return baseUrl.Replace(uri.Host, mappedDomain, StringComparison.OrdinalIgnoreCase);
    }
    return url; // ← return the original, untouched URL
}
}
