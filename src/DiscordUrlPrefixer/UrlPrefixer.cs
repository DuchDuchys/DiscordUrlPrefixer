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

    public static (string TransformedMessage, bool HadMatches) ReplaceUrls(string message)
    {
        var hadMatches = false;
        var result = message;

        foreach (var pattern in new[] { InstagramPattern(), TikTokPattern() })
        {
            var replaced = pattern.Replace(result, match =>
            {
                hadMatches = true;
                return SchemePrefix().Replace(match.Value, "$1kk", 1);
            });
            result = replaced;
        }

        return (result, hadMatches);
    }
}
