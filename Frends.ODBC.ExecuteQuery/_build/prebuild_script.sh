#!/bin/bash
echo $PWD
sudo su
sudo apt-get update
sudo apt-get -y install curl
sudo apt-get -y install gnupg
sudo apt-get -y install mdbtools
sudo apt-get -y install odbcinst
sudo apt-get -y install lsb-release

curl -sSL https://packages.microsoft.com/keys/microsoft.asc | sudo tee /etc/apt/trusted.gpg.d/microsoft.asc > /dev/null

curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list > ./mssql-release.list

sudo mv ./mssql-release.list /etc/apt/sources.list.d/mssql-release.list
sudo apt-get update
sudo ACCEPT_EULA=Y apt-get install -y --allow-unauthenticated msodbcsql17
# optional: for bcp and sqlcmd
sudo ACCEPT_EULA=Y apt-get install -y --allow-unauthenticated mssql-tools
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bash_profile
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

# optional: for unixODBC development headers
sudo apt-get install -y unixodbc-dev