FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["ExpressedRealms.Server/ExpressedRealms.Server.csproj", "ExpressedRealms.Server/"]
COPY ["ExpressedRealms.DB/ExpressedRealms.DB.csproj", "ExpressedRealms.DB/"]
RUN dotnet restore "ExpressedRealms.Server/ExpressedRealms.Server.csproj"
COPY . .
WORKDIR "/src/ExpressedRealms.Server"
RUN dotnet build "ExpressedRealms.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "ExpressedRealms.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK --interval=15s --timeout=60s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider https://0.0.0.0:5001/login || exit 1

USER $APP_UID
ENTRYPOINT ["dotnet", "ExpressedRealms.Server.dll"]
