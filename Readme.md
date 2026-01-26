[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Society-In-Shadow_ExpressedRealms&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Society-In-Shadow_ExpressedRealms)
[![Primary Workflow](https://github.com/Society-In-Shadow/ExpressedRealms/actions/workflows/DeployToProd.yml/badge.svg)](https://github.com/Society-In-Shadow/ExpressedRealms/actions/workflows/DeployToProd.yml)

# Welcome to Expressed Realms

Expressed Realms is the digital companion guide for Six Stones - Society in Shadows, a Capstone LARP System.

There are three main goals for this project

- Provide a cms system to store all the lore, expressions, game mechanics, and other information about the Society
- Provide a platform for creating and maintaining character sheets for the residents of the Society
- Provide potential employers a view into how I work on a production grade app (more targeted info can be found in [Prospective Employers](./documentation/prospectiveEmployers.md))

To get the full experience, feel free to create a user at [https://societyinshadows.org](https://society-in-shadows.com/)

# Architecture

## Quick Specs

- **Numbers** 80+ tables, 170+ API endpoints, 600+ unit tests, and 49,000+ lines of code
- **Frontend:** Vue 3 + Vite, TypeScript, Axios, Vue Router
- **Backend:** .NET 10 Minimal API, EF Core, PostgreSQL
- **Infrastructure:** Azure Container Apps, Blob Storage, Azure Key Vault
- **CI/CD:** GitHub Actions for build and deploy
- **Security:** Sonar Qube, MegaLinter
- **Logging:** Serilog with PostgreSQL sink, Application Insights, Audit.NET for User Logging

## More In Depth

In addition to the quick start below, the high level architecture and technologies can be found in the [architecture file](/documentation/architecture.md)

# Quick Start

## Prerequisites
- Container Runtime (Pick One)
  - [Docker Desktop](https://www.docker.com/products/docker-desktop)
  - [Podman](https://podman.io/) (Preferred choice)
- [.NET 10 SDK](https://dotnet.microsoft.com/download/)
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)
  - [Podman / Docker Integration](https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-with-podman/)

### Windows vs Linux
This project was primarily built using JetBrains Rider on Linux.  Some initial effort has been made to make it work on
Windows, though that integration has not been a primary focus as of late.

Scripts were written with podman in mind, conversion to docker should be as simple as changing the podman commands to docker.

## Setup the .env File

You only really need to do this if you are running linux.  The env file provides a spot to define user profile directory,
which is used to store the SSL certs.

Side Note: Avoid spaces on the right hand side of the values

```ini
USERPROFILE="/home/<user>"
```

## Create SSL Certs

Next, you need to setup SSL Certs

- Windows users can follow the instructions in [windows setup](/documentation/windowsSetup.md)
- Fedora users can follow the instructions in [fedora setup](/documentation/fedoraSetup.md)

## Data Population Scripts

These are stored separate if you would like access, ask about it in the discord group

## Start the Website

Once you get docker up and running, get the population scripts, add the .env file if you are on linux, and get the SSL certs
setup, you should be good to go to start the website.

Details on how the site runs can be found here: [docker-podman](/documentation/docker-podman.md)

But for initial setup, you want to get this working in docker, as you can see the progress of all the images downloading.

### Docker

What you want to do is open up the root of the project (the same directory as this readme) with command line or terminal,
and go into the "api" directory.

There you want to run the following command if you are on linux.

```shell
rebuildWeb.sh
```

Once that is done running, you should have a fully functional website

### Visual Studio / Rider

Visual Studio / Rider will automatically run the related docker commnands before build in order to fully run the site.

Once it's done building, you should have a fully functional website.

If you do choose to run this first, the first build will take a very long time to complete, as it needs to download all
the dependencies mentioned above.

## Local Links

Links to various places locally can be found here:

- [Front End / Web App](https://localhost:3001/)
- [Back End / Web API / Scalar](https://localhost:8443/scalar/)
- [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)
- [Feature Flags](http://localhost:8050)
- [Email Testing](http://localhost:8025)
