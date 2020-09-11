#!/bin/bash
# Set the service group name
export resource_group=handsOnAppService
# Set the plan
export plan=apicoreServicePlan
# Set the service name
export app_service_name=apicore-srv
# Set the api ASPNETCORE_ENVIRONMENT variables
export environment=StageAppServices
# Defines the ACR registry URL
export registry_address=webapicore.azurecr.io

# Create the app service
az webapp create --resource-group ${resource_group} \
                             --plan ${plan} \
                             --name ${app_service_name} \
                             --deployment-container-image-name ${registry_address}/myprimerwebapi_web_api:latest
                             
# Set the ASPNETCORE_ENVIRONMENT variable
az webapp config appsettings set -g ${resource_group} \
                                             -n ${app_service_name} \
                                             --settings ASPNETCORE_ENVIRONMENT=${environment}