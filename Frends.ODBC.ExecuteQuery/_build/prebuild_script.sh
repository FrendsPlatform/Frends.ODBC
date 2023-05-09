#!/usr/bin/env bash
echo $PWD
sudo su
sudo apt-get update
sudo apt-get -y install curl
sudo apt-get -y install gnupg
sudo apt-get -y install mdbtools
sudo apt-get -y install odbcinst
sudo apt-get -y install lsb-release

sudo su
curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -

curl https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/prod.list > ./mssql-release.list
exit

sudo cp -f ./mssql-release.list /etc/apt/sources.list.d/
sudo ACCEPT_EULA=Y apt-get install -y msodbcsql18
# optional: for bcp and sqlcmd
sudo ACCEPT_EULA=Y apt-get install -y mssql-tools18
echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc
source ~/.bashrc
# optional: for unixODBC development headers
sudo apt-get install -y unixodbc-dev
sudo cp -f ./_build/odbcinst.ini /etc/