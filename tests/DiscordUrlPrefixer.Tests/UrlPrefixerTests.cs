using DiscordUrlPrefixer;

namespace DiscordUrlPrefixer.Tests;

public class UrlPrefixerTests
{
    [Theory]
    [InlineData("https://www.instagram.com/reel/DV-8kcYiBG6/", "https://www.kkinstagram.com/reel/DV-8kcYiBG6/")]
    [InlineData("https://instagram.com/reel/DV-8kcYiBG6/", "https://kkinstagram.com/reel/DV-8kcYiBG6/")]
    [InlineData("https://www.instagram.com/reel/DV-8kcYiBG6/?igsh=MWd0MTZldWFnZzE2Yw==", "https://www.kkinstagram.com/reel/DV-8kcYiBG6/")]
    [InlineData("https://www.instagram.com/reels/abc123/", "https://www.kkinstagram.com/reels/abc123/")]
    [InlineData("https://www.instagram.com/p/xyz789/", "https://www.kkinstagram.com/p/xyz789/")]
    [InlineData("https://www.instagram.com/tv/video123/", "https://www.kkinstagram.com/tv/video123/")]
    public void InstagramUrl_TransformsCorrectly(string input, string expected)
    {
        var result = UrlPrefixer.TransformUrl(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("https://www.tiktok.com/@user/video/123456789", "https://www.kktiktok.com/@user/video/123456789")]
    [InlineData("https://tiktok.com/@user.123/video/987654321", "https://kktiktok.com/@user.123/video/987654321")]
    [InlineData("https://www.tiktok.com/@user/video/123456789?lang=en", "https://www.kktiktok.com/@user/video/123456789")]
    public void TikTokUrl_TransformsCorrectly(string input, string expected)
    {
        var result = UrlPrefixer.TransformUrl(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void NoUrls_ReturnsUnchanged()
    {
        var input = "Hello world, no links here";
        var (result, hadMatches) = UrlPrefixer.ReplaceUrls(input);
        Assert.False(hadMatches);
        Assert.Equal(input, result);
    }

    [Fact]
    public void MultipleUrls_BothReplaced()
    {
        var input = "https://www.instagram.com/reel/abc/ and https://www.tiktok.com/@user/video/123";
        var (result, hadMatches) = UrlPrefixer.ReplaceUrls(input);
        Assert.True(hadMatches);
        Assert.Contains("kkinstagram", result);
        Assert.Contains("kktiktok", result);
    }
}
