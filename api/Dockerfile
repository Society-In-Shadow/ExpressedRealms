FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["ExpressedRealms.sln", "."]
COPY ["ExpressedRealms.Repositories.Shared/ExpressedRealms.Repositories.Shared.csproj", "ExpressedRealms.Repositories.Shared/"]
COPY ["ExpressedRealms.Email/ExpressedRealms.Email.csproj", "ExpressedRealms.Email/"]
COPY ["ExpressedRealms.Repositories.Characters/ExpressedRealms.Repositories.Characters.csproj", "ExpressedRealms.Repositories.Characters/"]
COPY ["ExpressedRealms.Repositories.Expressions/ExpressedRealms.Repositories.Expressions.csproj", "ExpressedRealms.Repositories.Expressions/"]
COPY ["ExpressedRealms.DB/ExpressedRealms.DB.csproj", "ExpressedRealms.DB/"]
COPY ["ExpressedRealms.Server/ExpressedRealms.Server.csproj", "ExpressedRealms.Server/"]
RUN dotnet restore

COPY . .
WORKDIR "/src/ExpressedRealms.Server"

RUN dotnet publish "ExpressedRealms.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y --no-install-recommends  wget=1.21.3-1+b2 && rm -rf /var/lib/apt/lists/*

HEALTHCHECK --interval=15s --timeout=60s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://0.0.0.0:8080/health || exit 1

USER $APP_UID
ENTRYPOINT ["dotnet", "ExpressedRealms.Server.dll"]
