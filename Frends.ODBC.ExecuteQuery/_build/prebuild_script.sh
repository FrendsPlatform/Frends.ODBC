#!/usr/bin/env

mkdir -p  /etc/odbc
apt-get update
apt-get -y install sudo
apt-get -y install nano
apt-get -y install curl
apt-get -y install gnupg
apt-get -y install mdbtools
apt-get -y install odbcinst
apt update
apt -y install dos2unix
apt -y install lsb-release

if ! [[ "18.04 20.04 22.04" == *"$(lsb_release -rs)"* ]];
then
    echo "Ubuntu $(lsb_release -rs) is not currently supported.";
    exit;
fi

sudo su
curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -

curl https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/prod.list > /etc/apt/sources.list.d/mssql-release.list

sudo ACCEPT_EULA=Y apt-get install -y msodbcsql18
# optional: for bcp and sqlcmd
sudo ACCEPT_EULA=Y apt-get install -y mssql-tools18
echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc
source ~/.bashrc
# optional: for unixODBC development headers
sudo apt-get install -y unixodbc-dev

cp ./odbcinst.init ../../../../../../etc/odbcinst.init

