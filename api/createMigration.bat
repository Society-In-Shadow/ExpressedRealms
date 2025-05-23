@echo off
set /p MIGRATION_NAME="Enter migration name: "
dotnet ef migrations add %MIGRATION_NAME% --project ExpressedRealms.DB --startup-project ExpressedRealms.MigrationProject
pause

