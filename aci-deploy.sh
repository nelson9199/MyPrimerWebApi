#!/bin/bash
# Set the service group name
export resource_group=handsOnContainerServices
# Set the registry address
export registry_address=apicontainer99.azurecr.io
# Set the registry username
export registry_username=apiContainer99
# Set the registry password
export registry_password=nXFZafFRoGgR06Q6YWXhaM=HODDxuttg
# Set the sql container name
export sql_name=ms-sql-server
# Set the sql admin password
export sql_admin_pass=Pa55w0rd2019
# Set the service name
export api_name=mywebapi99


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
                    --image ${registry_address}/web-api:v1 \
                    --cpu 1 \
                    --memory 1 \
                    --dns-name-label ${api_name} \
                    --ports 80 \
                    --ip-address public \
                    --registry-password=${registry_password} \
                    --registry-username=${registry_username
