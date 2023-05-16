FROM ubuntu:22.04 AS builder

RUN apt-get update && apt-get upgrade -y && apt-get install -y dotnet6 ca-certificates

RUN DEBIAN_FRONTEND=noninteractive apt-get install -y \
        ca-certificates \
        \
        # Dependencies
        curl \
        sudo \
        dos2unix \
        gnupg \
        mdbtools \
        odbcinst \
        lsb-release \
        unixodbc-dev

RUN sudo curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
RUN curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list > ./mssql-release.list
RUN sudo mv ./mssql-release.list /etc/apt/sources.list.d/mssql-release.list
RUN apt-get update
RUN sudo ACCEPT_EULA=Y apt-get install -y --allow-unauthenticated msodbcsql17 unixodbc-dev
RUN sudo ACCEPT_EULA=Y apt-get install -y --allow-unauthenticated mssql-tools
RUN echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bash_profile
RUN echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app
COPY ./prebuild_script.sh /usr/src/app/prebuild_script.sh
COPY ./odbcinst.ini /etc/odbcinst.ini
RUN dos2unix /etc/odbcinst.ini

#RUN chmod +x /usr/src/app/prebuild_script.sh
#RUN dos2unix /usr/src/app/prebuild_script.sh

EXPOSE 80 443

#RUN /usr/src/app/prebuild_script.sh

RUN echo "Successful"
ENTRYPOINT ["tail", "-f", "/dev/null"]
