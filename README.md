
# Lion

## About and Status
Lion is a .NET Core Web API that provides a RESTful interface for interacting with localized resources.

As of this writing, the project is a work in progress.

## Quickstart
1. Clone the repo:
```
git clone https://github.com/jamesrm9235/Lion.git
```
2. Build the images:
```
docker compose build
```
3. Run the containers:
```
docker compose -f docker-compose.yml up
```
4. Send a request to http://localhost:5000/api to verify the web service is running.

### Run with HTTPS
The quickstart above runs the service without HTTPS.
If you would like to test with HTTPS, follow these steps:

1. Follow [this](https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-5.0#running-pre-built-container-images-with-https) guide to generate a certificate with a password.

2. Update `docker-compose.override.yml`:

    a CERT_PASSWORD to the password used when the certificate was generated

    b CERT_EXPORT_PATH to the certificate's exported path

3. Run the containers:
```
docker compose up
```