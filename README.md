# DiscordUrlPrefixer

Prefixes Instagram and TikTok URLs with "kk" for Discord bots. Automatically strips query parameters (e.g., `?igsh=...`).

## Usage

```csharp
var (result, hadMatches) = UrlPrefixer.ReplaceUrls("https://instagram.com/reel/abc/?igsh=123");
// result: "https://kkinstragram.com/reel/abc/"
```

## Build & Test

```bash
dotnet build
dotnet test
```
