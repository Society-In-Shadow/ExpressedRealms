#!/bin/bash

# Script to stop, build, and restart a specific Docker Compose container

CONTAINER_NAME="webapi"

./syncPermissions.sh

echo "🧹 Removing old Images"

podman image prune -f

echo "🔨 Building container: $CONTAINER_NAME"

if ! podman compose -f ../docker-compose.yaml -f ../docker-compose.container-api.yaml build "$CONTAINER_NAME" --no-cache; then
  echo "❌ Failed to build container"
  exit 1
fi

echo "🛑 Stopping container: $CONTAINER_NAME"

if ! podman compose -f ../docker-compose.yaml -f ../docker-compose.container-api.yaml stop "$CONTAINER_NAME"; then
  echo "❌ Failed to stop container"
  exit 1
fi

echo "🚀 Starting container: $CONTAINER_NAME"

if ! podman compose -f ../docker-compose.yaml -f ../docker-compose.container-api.yaml up -d; then
  echo "❌ Failed to start container"
  exit 1
fi
