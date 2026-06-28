#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "🗘 Syncing Permissions and Feature Flags..."

dotnet publish "$SCRIPT_DIR/ExpressedRealms.PermissionSync" \
  -o "$SCRIPT_DIR/ExpressedRealms.PermissionSync/bin/output" -v q

"$SCRIPT_DIR/ExpressedRealms.PermissionSync/bin/output/ExpressedRealms.PermissionSync" \
  >"$SCRIPT_DIR/../client/src/types/UserPermissions.ts"

echo "✅ Synced Permissions!"

dotnet publish "$SCRIPT_DIR/ExpressedRealms.Shared.FeatureFlags.UiSync" \
  -o "$SCRIPT_DIR/ExpressedRealms.Shared.FeatureFlags.UiSync/bin/output" -v q

"$SCRIPT_DIR/ExpressedRealms.Shared.FeatureFlags.UiSync/bin/output/ExpressedRealms.Shared.FeatureFlags.UiSync" \
  >"$SCRIPT_DIR/../client/src/types/FeatureFlags.ts"
  
echo "✅ Synced Feature Flags!"