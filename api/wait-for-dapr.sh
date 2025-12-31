#!/bin/sh
# wait-for-dapr.sh
set -e

echo "Waiting for Dapr sidecar to be ready..."
while ! nc -z dapr-sidecar 50001; do
  echo "Dapr not ready yet, sleeping 1s..."
  sleep 1
done

echo "Dapr is up! Starting web API..."
exec dotnet ExpressedRealms.Server.dll