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

    [Fact]
    public void NoUrls_ReturnsUnchanged()
    {
        var input = "Hello world, no links here";
        var (result, hadMatches) = UrlPrefixer.ReplaceUrls(input);
        Assert.False(hadMatches);
        Assert.Equal(input, result);
    }

    [Fact]
    public void MultipleInstagramLinks_Replaced()
    {
        var input = "https://www.instagram.com/reel/abc/ https://www.instagram.com/p/xyz/";
        var (result, hadMatches) = UrlPrefixer.ReplaceUrls(input);
        Assert.True(hadMatches);
        Assert.Contains("kkinstagram", result);
    }

    [Theory]
    [InlineData("https://x.com/AndreasSteno/status/2034357635256922473", "https://girlcockx.com/AndreasSteno/status/2034357635256922473")]
    [InlineData("https://www.x.com/AndreasSteno/status/2034357635256922473", "https://girlcockx.com/AndreasSteno/status/2034357635256922473")]
    [InlineData("https://x.com/user/status/123?param=value", "https://girlcockx.com/user/status/123")]
    [InlineData("https://twitter.com/AndreasSteno/status/2034357635256922473", "https://girlcockx.com/AndreasSteno/status/2034357635256922473")]
    [InlineData("https://www.twitter.com/AndreasSteno/status/2034357635256922473", "https://girlcockx.com/AndreasSteno/status/2034357635256922473")]
    [InlineData("https://twitter.com/user/status/123?param=value", "https://girlcockx.com/user/status/123")]
    public void TwitterXUrl_TransformsCorrectly(string input, string expected)
    {
        var result = UrlPrefixer.TransformUrl(input);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("https://github.com/user/repo")]
    [InlineData("https://google.com/search?q=test")]
    [InlineData("https://youtube.com/watch?v=abc123")]
    [InlineData("https://reddit.com/r/programming")]
    [InlineData("https://example.com/some/path?query=value")]
    [InlineData("https://stackoverflow.com/questions/12345/some-question")]
    [InlineData("https://discord.com/channels/123/456")]
    [InlineData("https://some-random-site.org/page")]
    public void RandomUrls_NotChanged(string input)
    {
        var result = UrlPrefixer.TransformUrl(input);
        Assert.Equal(input, result);
    }

    [Fact]
    public void MixedUrls_OnlyTwitterAndInstagramTransformed()
    {
        var input = "Check this https://x.com/user/status/123 and this https://github.com/repo and https://instagram.com/p/abc/";
        var (result, hadMatches) = UrlPrefixer.ReplaceUrls(input);
        Assert.True(hadMatches);
        Assert.Contains("girlcockx.com", result);
        Assert.Contains("github.com", result);
        Assert.Contains("kkinstagram.com", result);
    }

    [Theory]
    [InlineData("https://github.com/user/repo?tab=readme", "https://github.com/user/repo?tab=readme")]
    [InlineData("https://google.com/search?q=test&sort=relevance", "https://google.com/search?q=test&sort=relevance")]
    [InlineData("https://youtube.com/watch?v=abc123&t=10s", "https://youtube.com/watch?v=abc123&t=10s")]
    [InlineData("https://reddit.com/r/programming?ref=home", "https://reddit.com/r/programming?ref=home")]
    [InlineData("https://discord.com/channels/123/456?some=param", "https://discord.com/channels/123/456?some=param")]
    [InlineData("https://example.com/path?a=1&b=2&c=3", "https://example.com/path?a=1&b=2&c=3")]
    public void RandomUrlsWithQueries_NotChanged(string input, string expected)
    {
        var result = UrlPrefixer.TransformUrl(input);
        Assert.Equal(expected, result);
    }
}
