FROM ubuntu:22.04 AS builder

RUN apt-get update && apt-get install -y dotnet6 ca-certificates

RUN DEBIAN_FRONTEND=noninteractive apt-get install -y \
        ca-certificates \
        \
        # Dependencies
        curl \
        sudo \
        dos2unix

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app
COPY ./prebuild_script.sh /usr/src/app/prebuild_script.sh
COPY ./odbcinst.ini /etc/odbcinst.ini
RUN dos2unix /etc/odbcinst.ini

RUN chmod +x /usr/src/app/prebuild_script.sh
RUN dos2unix /usr/src/app/prebuild_script.sh

EXPOSE 80 443

RUN /usr/src/app/prebuild_script.sh

RUN echo "Successful"
ENTRYPOINT ["tail", "-f", "/dev/null"]
