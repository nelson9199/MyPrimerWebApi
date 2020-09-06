#!/bin/bash

# Set an user login and password for your database
export user_admin=webapi_srv
export user_pass=P@ssw0rd
# The logical server name has to be unique in the system
export server_name=bibliotecawebapi
# Set the database name
export database_name=Biblioteca
# The resource group name
export resourceGroup=handsOnAppService
# The ip address range that you want to allow to access your DB
export startip=0.0.0.0
export endip=0.0.0.0

# Create a resource group
az group create \
   --name ${resourceGroup} \
   --location eastus

# Create a logical server in the resource group
az sql server create \
   --name ${server_name} \
   --resource-group ${resourceGroup} \
   --location eastus  \
   --admin-user ${user_admin} \
   --admin-password ${user_pass}

# Configure a firewall rule and open to local Azure services
az sql server firewall-rule create \
   --resource-group ${resourceGroup} \
   --server ${server_name} \
   -n AllowYourIp \
   --start-ip-address ${startip} \
   --end-ip-address ${endip}

# Create a database in the server 
az sql db create \
   --resource-group ${resourceGroup} \
   --server ${server_name} \
   --name ${database_name} \
   --service-objective S0 \
    --zone-redundant false