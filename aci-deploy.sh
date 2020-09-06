#!/bin/bash
# Set the service group name
export resource_group=handsOnContainerServices
# Set the registry address
export registry_address=webapicore.azurecr.io
# Set the registry username
export registry_username=webapicore
# Set the registry password
export registry_password=imLkRFqAt/2cVSW8zeex14nkL4WHon3Y
# Set the sql container name
export sql_name=api-db
# Set the sql admin password
export sql_admin_pass=Nelson_9191
# Set the service name
export api_name=web-api
# Set the api ASPNETCORE_ENVIRONMENT variables
export environment=Stage

az container create --resource-group ${resource_group} \
                    --location eastus \
                    --name ${sql_name} \
                    --image microsoft/mssql-server-linux \
                    --cpu 1 \
                    --memory 1 \
                    --dns-name-label ${sql_name} \
                    --ports 1433 \
                    --environment-variables ACCEPT_EULA=Y SA_PASSWORD=${sql_admin_pass}              

az container create --resource-group ${resource_group} \
                    --location eastus \
                    --name ${api_name} \
                    --image ${registry_address}/biblioteca_web_api:v1 \
                    --cpu 1 \
                    --memory 1 \
                    --dns-name-label ${api_name} \
                    --ports 80 \
                    --ip-address public \
                    --environment-variables ASPNETCORE_ENVIRONMENT=${environment} \
                    --registry-password=${registry_password} \
                    --registry-username=${registry_username}
