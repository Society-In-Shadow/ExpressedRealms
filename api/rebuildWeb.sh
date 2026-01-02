#!/bin/bash

# Script to stop, build, and restart a specific Docker Compose container
# Usage: ./restart-container.sh

CONTAINER_NAME="webapi"

echo "🗘 Syncing Permissions..."

dotnet publish ./ExpressedRealms.PermissionSync -o ./ExpressedRealms.PermissionSync/bin/output -v q
./ExpressedRealms.PermissionSync/bin/output/ExpressedRealms.PermissionSync > ./../client/src/types/Permissions.ts

echo "✅ Synced Permissions!"

echo "🔨 Building container: $CONTAINER_NAME"

if ! podman compose -f ../docker-compose.yaml -f ../docker-compose.container-api.yaml build "$CONTAINER_NAME"; then
  echo "❌ Failed to build container"
  exit 1
fi

echo "🛑 Stopping container: $CONTAINER_NAME"

podman compose stop "dapr-sidecar"
podman compose stop "myapp"

if ! podman compose -f ../docker-compose.yaml -f ../docker-compose.container-api.yaml stop "$CONTAINER_NAME"; then
  echo "❌ Failed to stop container"
  exit 1
fi

echo "🚀 Starting container: $CONTAINER_NAME"

if ! podman compose -f ../docker-compose.yaml -f ../docker-compose.container-api.yaml up -d; then
  echo "❌ Failed to start container"
  exit 1
fi
