FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
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
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Install netcat (Debian-based)
RUN apt-get update \
    && apt-get install -y netcat-openbsd \
    && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=build /app/publish .

# Copy the wait script
COPY wait-for-dapr.sh /app/wait-for-dapr.sh
RUN chmod +x /app/wait-for-dapr.sh

HEALTHCHECK --interval=15s --timeout=60s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://0.0.0.0:8080/health || exit 1

USER $APP_UID

# Use the wait script as entrypoint
ENTRYPOINT ["/app/wait-for-dapr.sh"]
