#!/bin/bash
read -r -p "Enter migration name (CamelCase): " MIGRATION_NAME
dotnet ef migrations add "$MIGRATION_NAME" --project ExpressedRealms.DB --startup-project ExpressedRealms.MigrationProject
