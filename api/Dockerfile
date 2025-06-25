FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["ExpressedRealms.sln", "./"]
# https://stackoverflow.com/a/51373386
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done

RUN dotnet restore

COPY . .
WORKDIR "/src/ExpressedRealms.Server"

RUN dotnet publish "ExpressedRealms.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y --no-install-recommends  wget=1.21.3-1+deb12u1 && rm -rf /var/lib/apt/lists/*

HEALTHCHECK --interval=15s --timeout=60s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://0.0.0.0:8080/health || exit 1

USER $APP_UID
ENTRYPOINT ["dotnet", "ExpressedRealms.Server.dll"]
