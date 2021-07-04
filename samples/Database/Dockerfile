# Based on the example here: https://github.com/twright-msft/mssql-node-docker-demo-app
FROM mcr.microsoft.com/mssql/server:2017-CU24-ubuntu-16.04

# Switch to root user
USER root

# Create app directory
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

# Copy scripts
COPY . /usr/src/app

# Grant permissions for the create-db script to be executable
RUN chmod +x /usr/src/app/create-db.sh

EXPOSE 1433

CMD /bin/bash ./entrypoint.sh