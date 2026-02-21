FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src

COPY src/DiscordUrlPrefixer/DiscordUrlPrefixer.csproj src/DiscordUrlPrefixer/
RUN dotnet restore src/DiscordUrlPrefixer/DiscordUrlPrefixer.csproj

COPY src/ src/
RUN dotnet publish src/DiscordUrlPrefixer/DiscordUrlPrefixer.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine
WORKDIR /app

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

RUN addgroup -S appgroup && adduser -S appuser -G appgroup
USER appuser

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DiscordUrlPrefixer.dll"]
