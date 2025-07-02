#!/usr/bin/env bash
set -e

RESOURCE_GROUP=$1
WEBAPP_NAME=$2
LOCATION=${3:-eastus}

if [ -z "$RESOURCE_GROUP" ] || [ -z "$WEBAPP_NAME" ]; then
  echo "Usage: deploy.sh <resource-group> <webapp-name> [location]"
  exit 1
fi

az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

az deployment group create \
  --resource-group "$RESOURCE_GROUP" \
  --template-file azuredeploy.json \
  --parameters webAppName="$WEBAPP_NAME"

zip -r app.zip EmployeeManagementApi -x "*/bin/*" "*/obj/*"

az webapp deploy \
  --resource-group "$RESOURCE_GROUP" \
  --name "$WEBAPP_NAME" \
  --src-path app.zip \
  --type zip
