# syntax=docker/dockerfile:1

# Create a stage for building the application.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
ARG TARGETARCH

COPY . /source

WORKDIR /source/ContentManagement/ContentAPI

# Build the application.
RUN dotnet publish -o /app

# Create a stage for running the application.
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS final
WORKDIR /app

# Copy everything needed to run the app from the "build" stage.
COPY --from=build /app .

# Create a non-privileged user that the app will run under.
# See https://docs.docker.com/develop/develop-images/dockerfile_best-practices/#user
ARG UID=1001
RUN adduser \
    --disabled-password \
    --gecos "" \
    --home "/nonexistent" \
    --shell "/sbin/nologin" \
    --no-create-home \
    --uid "${UID}" \
    appuser

# Set environment variables.
ENV ASPNETCORE_URL http://+:8080

# Give User 1001 ownership and access to /app
RUN chown -R 1001:0 /app && chmod -R og+rwx /app

# Expose the port.
EXPOSE 8080

# Run container by default as user with id 1001 (default)
USER appuser

ENTRYPOINT ["dotnet", "ContentAPI.dll"] 
