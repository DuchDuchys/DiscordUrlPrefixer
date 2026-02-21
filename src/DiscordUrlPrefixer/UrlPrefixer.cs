using System.Text.RegularExpressions;

namespace DiscordUrlPrefixer;

public static partial class UrlPrefixer
{
    // Matches Instagram URLs: /reel/, /reels/, /p/, /tv/
    [GeneratedRegex(@"https?://(?:www\.)?instagram\.com/(?:reel|reels|p|tv)/[\w-]+/?[^\s]*", RegexOptions.IgnoreCase)]
    private static partial Regex InstagramPattern();

    // Matches TikTok URLs: /@user/video/id
    [GeneratedRegex(@"https?://(?:www\.)?tiktok\.com/@[\w.-]+/video/\d+[^\s]*", RegexOptions.IgnoreCase)]
    private static partial Regex TikTokPattern();

    // Captures the scheme + optional www. prefix, then the domain
    [GeneratedRegex(@"^(https?://(?:www\.)?)")]
    private static partial Regex SchemePrefix();

    public static List<(string Original, string Prefixed)> ExtractAndPrefix(string message)
    {
        var results = new List<(string Original, string Prefixed)>();

        foreach (var pattern in new[] { InstagramPattern(), TikTokPattern() })
        {
            foreach (Match match in pattern.Matches(message))
            {
                var original = match.Value;
                // Insert "kk" after the scheme+www prefix, before the domain name
                // e.g. https://www.instagram.com/... → https://www.kkinstagram.com/...
                //      https://tiktok.com/...       → https://kktiktok.com/...
                var prefixed = SchemePrefix().Replace(original, "$1kk", 1);
                results.Add((original, prefixed));
            }
        }

        return results;
    }
}
