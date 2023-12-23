FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Security related, prevent docker from running as root user
USER $APP_UID 
WORKDIR /app
EXPOSE 8080

# Ensure we listen on any IP Address 
ENV DOTNET_URLS=http://+:5000

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

ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "ExpressedRealms.Server.dll"]
