#!/bin/bash

dotnet pack -c Release

dotnet tool uninstall ExpressedRealms.Scaffolder --global 2>/dev/null || true

dotnet tool install -g ExpressedRealms.Scaffolder \
  --add-source ./bin/Release