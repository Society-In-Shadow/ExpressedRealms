# Docker Desktop

Docker is required to run this application locally.

Download and install [Docker Desktop](https://www.docker.com/products/docker-desktop/).
Follow their instructions to get docker up and running : [Install Windows](https://docs.docker.com/desktop/install/windows-install/#install-docker-desktop-on-windows)

Once you have their hello world example up and running, you should be good to go.

## Docker Commands

### To start the application
```shell
docker compose up
```

### To stop the application
```shell
docker compose down
```

### To rebuild everything
```shell
docker compose build --no-cache
```