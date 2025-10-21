FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /src

# https://stackoverflow.com/a/51373386
COPY ExpressedRealms.sln ./
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv "$file" ${file%.*}/; done
RUN dotnet restore ExpressedRealms.sln

# Stage 2: Build + publish
FROM restore AS build
COPY . .
WORKDIR /src/ExpressedRealms.Server
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false --no-restore -m:"$(nproc)"

# Stage 3: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y --no-install-recommends \
    wget=1.21.3-1+deb12u1 \
    fonts-dejavu-core=2.37-6 \
    fonts-liberation=1:1.07.4-11 \
    fontconfig=2.14.1-4 \
    && fc-cache -f -v \
    && rm -rf /var/lib/apt/lists/*

HEALTHCHECK --interval=15s --timeout=60s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://0.0.0.0:8080/health || exit 1

USER $APP_UID
ENTRYPOINT ["dotnet", "ExpressedRealms.Server.dll"]
