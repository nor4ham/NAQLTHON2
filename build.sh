#!/bin/bash

# Build the Docker image for the ASP.NET Core application
docker build -t garage .

# Recreate and restart the Docker Compose services
docker-compose down
docker-compose up -d
