#!/bin/bash

# If the all docker version was running, we need to switch the password
# store to one that is compatible with running locally
podman compose stop "dapr-sidecar"
podman compose stop "myapp"

podman compose up -d