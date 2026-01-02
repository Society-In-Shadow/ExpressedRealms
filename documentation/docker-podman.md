# Docker Desktop / Podman

## Docker Desktop vs Podman

### Docker Desktop
Docker Desktop is the biggest commercially available application that allows you to run Docker containers locally.
It has a free tier, for companies that earn less than 10 million USD per year
It's widely used on Window Machines.

Can be downloaded here
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Podman
Podman is an open source alternative to Docker Desktop.  It was built by Red Hat, and integrates better with Linux.

Podman was choosen over Docker Desktop because of the open source nature of it and better integration with Linux.

Can be downloaded here
- [Podman](https://podman.io/)

## Our Application
Due to above reasons, Podman is used for all scripts at least for the linux scripts.

Your primary focus should be getting their version of hello world working, all you need to do is run this command

```shell
podman run hello-world
```

or 

```shell
docker run hello-world
```

Once that is running without issue, your container environment is up and running.

## Running Docker / Podman with Our Application
There are two script files that you need to be aware of in the API folder.

### rebuildWeb.sh
This is specifically for running the whole website in via containers.  

It will:
- Sync API permissions to the client folder
- Build the docker image for the API
- Reset docker configuration to ensure that docker version of the API can run successfully

It is preferred to run this script initially, as you can see the progress of the dependencies being downloaded, which
takes quite a bit of time on intiial load.

It will also ensure that everything is up and running correctly.

### startServicesBeforeBuild.sh
This is specifically for running the API in an IDE of some sort.

This script isn't something you directly invoke, it's meant to be invoked automatically by the server project on build.

In the csproj, it's setup to trigger before the build process.

Like above, it will:
- Sync API permissions to the client folder
  - Reset docker configuration to ensure that IDE version of the API can run successfully
  1`2QQQ