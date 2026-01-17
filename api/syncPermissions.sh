#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "🗘 Syncing Permissions..."

dotnet publish "$SCRIPT_DIR/ExpressedRealms.PermissionSync" \
  -o "$SCRIPT_DIR/ExpressedRealms.PermissionSync/bin/output" -v q

"$SCRIPT_DIR/ExpressedRealms.PermissionSync/bin/output/ExpressedRealms.PermissionSync" \
  >"$SCRIPT_DIR/../client/src/types/UserPermissions.ts"

echo "✅ Synced Permissions!"
