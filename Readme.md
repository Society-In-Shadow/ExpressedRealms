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

The database will be persistent across the docker images build / rebuild.  

The actual physical location of the db is in "your user folder/expressedRealms/postgres"
If you want to reset it, just delete the folder

#### pgAdmin
pgAdmin is a postgres database management tool.  For Expressed Realms, it runs in a docker container.

When the app is started for the first time, it will create a new directory

All the data regarding it is stored in "your user folder/expressedRealms/pgAdmin" folder.

You can access here:
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)


##### Connect to DB

When you visit it, you will need to add a server.  Click add server.

To login:
* username: user-name@domain-name.com
* password: strong-password

On the popup
* General Tab
  * Name - Expressed Realms
* Connection Tab
  * Hostname/Address - expressed-realms-db
  * Port - 5432
  * Maintenance Database - expressedRealms
  * UserName - user
  * Password - password
  * Remember Password - Enable it

Hit save, and it should connect.

NOTE: I don't think you can connect to this from a local install of pgAdmin
think the hostname would be localhost

To test: On the left hand side, 
* Servers
  * Expressed Realms
    * Databases
      * ExpressedRealms
        * Schemas (2)
          * public
            * tables
              * characters

Right click on that table, and hit view / edit data.

There should be 2 characters in there, John and Jane Doe.

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