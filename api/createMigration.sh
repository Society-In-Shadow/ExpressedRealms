#!/bin/bash
if [ -n "$1" ]; then
    MIGRATION_NAME="$1"
else
    read -r -p "Enter migration name (CamelCase): " MIGRATION_NAME
fi

dotnet ef migrations add "$MIGRATION_NAME" --project ExpressedRealms.DB --startup-project ExpressedRealms.MigrationProject
