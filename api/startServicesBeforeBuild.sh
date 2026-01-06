#!/bin/bash

# Reminder that this is relative to the ExpressedRealms.Server folder, as that is where the root of this is at
podman compose -f ../../docker-compose.yaml -f ../../docker-compose.container-api.yaml stop "webapi"

podman compose up -d
