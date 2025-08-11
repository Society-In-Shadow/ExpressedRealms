#!/bin/bash

# Script to stop, build, and restart a specific Docker Compose container
# Usage: ./restart-container.sh

CONTAINER_NAME="webapi"

echo "🛑 Stopping container: $CONTAINER_NAME"

if ! podman compose stop "$CONTAINER_NAME"; then
  echo "❌ Failed to stop container"
  exit 1
fi

echo "🔨 Building container: $CONTAINER_NAME"

if ! podman compose build "$CONTAINER_NAME"; then
  echo "❌ Failed to build container"
  exit 1
fi

echo "🚀 Starting container: $CONTAINER_NAME"

if ! podman compose up -d "$CONTAINER_NAME"; then
  echo "❌ Failed to start container"
  exit 1
fi
