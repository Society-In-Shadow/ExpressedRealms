# Expressed Realms

Expressed Realms is awesome!

## Overview



### Quick Start

Download and install docker desktop, and get their welcome world sample running.

After that, open up the root of the project (the same directory as this readme) and run

```
docker compose up
```

That will spin up everything needed to run the application.

You will have 3 URLS:

* [Front End / Web App](http://localhost:5173/)
* [Back End / Web API / Swagger](http://localhost:8080/swagger/index.html)
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)

### Database Basics

#### Postgres
We use a postgres database.  On our locals, that db will be handled by the docker image for postgres.

On first start, the web api will populate the db and fill it in with sample data.

Connection details can be found in the docker-compose.yaml file.

#### pgAdmin
When the app is started for the first time, it will create a new directory

### Docker Commands

#### To start the application
```
docker compose up
```

#### To stop the application
```
docker compose down
```

#### To rebuild everything
```
docker compose build --no-cache
```

#### To Reset the DB

Delete the pgadmin-data directory.  This directory contains the physical DB.