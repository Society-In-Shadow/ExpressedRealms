FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Security related, prevent docker from running as root user
# Create a system group named "user" with the -r flag
RUN groupadd -g 10001 dotnet && \
   useradd -u 10000 -g dotnet dotnet \
   && chown -R dotnet:dotnet /app \
   && chown -R dotnet:dotnet /src

USER dotnet:dotnet

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

ENTRYPOINT ["dotnet", "ExpressedRealms.Server.dll"]
