name: Build and Deploy

on:
  push:
    branches:
      - main  # Trigger the workflow on pushes to the 'main' branch

jobs:
  build:
    runs-on: ubuntu-latest
    permissions:
      contents: read
      packages: write

    steps:
      - name: Checkout code
        uses: actions/checkout@v2

      - name: Setup .NET Core
        uses: actions/setup-dotnet@v1
        with:
          dotnet-version: 8.0.101  # Adjust the version as needed

      - name: Restore dependencies
        run: dotnet restore

      - name: Build application
        run: dotnet build --configuration Release

  docker:
    needs: build
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v2

      - name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v1

      - name: Build and Push Docker image
        uses: docker/build-push-action@v2
        with:
          context: .  # Use the current directory as the build context
          file: ./Dockerfile  # Path to your Dockerfile
          push: true  # Push the built image to the container registry
          tags: |
            docker.pkg.github.com/nor4ham/naqlthon2/naqlthon:latest
            docker.pkg.github.com/nor4ham/naqlthon2/naqlthon:${{ github.sha }}
        env:
          DOCKER_USERNAME: ${{ secrets.GHCR_USERNAME }}
          DOCKER_PASSWORD: ${{ secrets.GHCR_PAT }}  # Use the secret you created

  deploy:
    needs: docker
    runs-on: ubuntu-latest

    steps:
      - name: Deploy to Hosting Environment
        run: |
          # Add your deployment steps here
          # Example: deploy to a web server or a container orchestration platform
          # Replace this with your deployment script
          echo "Deploying to hosting environment..."
