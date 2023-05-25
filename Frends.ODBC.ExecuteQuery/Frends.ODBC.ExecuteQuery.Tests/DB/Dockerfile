FROM mcr.microsoft.com/mssql/server:2017-latest-ubuntu

ENV SA_PASSWORD yourStrong!Password
ENV ACCEPT_EULA Y
ENV MSSQL_PID Express

RUN apt-get update
RUN apt-get -y install dos2unix

RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app
COPY ./entrypoint.sh /usr/src/app/entrypoint.sh
COPY ./init-db.sql /usr/src/app/init-db.sql
COPY ./run-initialization.sh /usr/src/app/run-initialization.sh

RUN chmod +x /usr/src/app/run-initialization.sh
RUN dos2unix /usr/src/app/entrypoint.sh
RUN dos2unix /usr/src/app/run-initialization.sh

EXPOSE 1433

CMD /bin/bash /usr/src/app/entrypoint.sh