﻿#!/bin/bash
sudo apt-get update
sudo apt-get -y install sudo
sudo apt-get -y install nano
sudo apt-get -y install curl
sudo apt-get -y install gnupg
sudo apt-get -y install mdbtools
sudo apt-get -y install odbcinst
sudo apt update
sudo apt -y install dos2unix
sudo apt -y install lsb-release
if ! [[ "18.04 20.04 22.04" == *"$(lsb_release -rs)"* ]];
then
    echo "Ubuntu $(lsb_release -rs) is not currently supported.";
    exit;
fi
sudo su
sudo curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
sudo curl https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/prod.list > /etc/apt/sources.list.d/mssql-release.list
sudo ACCEPT_EULA=Y apt-get install -y msodbcsql18
# optional: for bcp and sqlcmd
sudo ACCEPT_EULA=Y apt-get install -y mssql-tools18
echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc
sudo source ~/.bashrc
# optional: for unixODBC development headers
sudo apt-get install -y unixodbc-dev
sudo cp ./odbcinst.init ../../../../../../etc/odbcinst.init
echo "file odbcinst.init copied to etc/odbcinst.init"