#!/bin/bash

# Script to stop, build, and restart a specific Docker Compose container
# Usage: ./restart-container.sh

CONTAINER_NAME="webapi"

echo "🛑 Stopping container: $CONTAINER_NAME"
docker-compose stop "$CONTAINER_NAME"

if [ $? -eq 0 ]; then
    echo "✅ Container stopped successfully"
else
    echo "❌ Failed to stop container"
    exit 1
fi

echo "🔨 Building container: $CONTAINER_NAME"
docker-compose build "$CONTAINER_NAME"

if [ $? -eq 0 ]; then
    echo "✅ Container built successfully"
else
    echo "❌ Failed to build container"
    exit 1
fi

echo "🚀 Starting container: $CONTAINER_NAME"
docker-compose up -d "$CONTAINER_NAME"

if [ $? -eq 0 ]; then
    echo "✅ Container started successfully"
else
    echo "❌ Failed to start container"
    exit 1
fi

echo "🎉 Container restart completed!"
