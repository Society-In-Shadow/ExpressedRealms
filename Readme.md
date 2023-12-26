# Expressed Realms

Expressed Realms is awesome!

## Quick Start

### Docker Desktop

Docker is required to run this application locally.

Download and install [Docker Desktop](https://www.docker.com/products/docker-desktop/).
Follow their instructions to get docker up and running : [Install Windows](https://docs.docker.com/desktop/install/windows-install/#install-docker-desktop-on-windows)

Once you have their hello world example up and running, you should be good to go.

### Configure DB Stuff

In the root folder (Same folder as this readme), you need to create a ".env" file.  In said file, add this information.
Side Note: Avoid spaces on the right hand side of the values

```
# This is the location on your local machine
# that will store the physical db. Details
# can be found in the DB section
DB_STORAGE_LOCATION=

# pgAdmin is the db management tool. 
# These values are your login credentials
PGADMIN_EMAIL=
PGADMIN_PASSWORD=

# This is the db name, plus the user and 
# password you need to connect to it
DB_NAME=expressedRealms
DB_USER=
DB_PASSWORD=

```

### Run Society in Shadows

Once you get docker up and running, and get the environment file in place, you should be good to go to start the website.

What you want to do is open up the root of the project (the same directory as this readme) in powershell, and run the 
following command.

```
docker compose up
```

It will start to do a lot of things.  If this is the first run, it will take some time to download stuff. 

Once everything has been downloaded, it should start db followed by the vue app.  Once the DB is up and running, it will 
start the web api, then the pgAdmin.

Once the messages cool down, you can visit links below

### Important Links

* [Front End / Web App](http://localhost:5173/)
* [Back End / Web API / Swagger](http://localhost:8080/swagger/index.html)
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)

## Database Basics

### Postgres
We use a postgres database.  On our locals, that db will be handled by the docker image for postgres.

On first start, the web api will populate the db and fill it in with sample data.

Connection details can be found in the docker-compose.yaml file.

The database will be persistent across the docker images build / rebuild.  

The actual physical location of the db is in "your user folder/expressedRealms/postgres"
If you want to reset it, just delete the folder

### pgAdmin
pgAdmin is a postgres database management tool.  For Expressed Realms, it runs in a docker container.

When the app is started for the first time, it will create a new directory

All the data regarding it is stored in "your user folder/expressedRealms/pgAdmin" folder.

You can access here:
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)


#### Connect to DB

When you visit it, it will prompt you for a username and password

To login, take a look at that ".env" file you created, it's the credentials you put there.

Once you get in, you need click add server.

On the popup, fill in the following values, some of which are from the ".env" file from earlier
* General Tab
  * Name - Expressed Realms
* Connection Tab
  * Hostname/Address - expressed-realms-db
  * Port - 5432
  * Maintenance Database - From the env file: DB_Name
  * UserName - From the env file: DB_User
  * Password - From the env file: DB_Password
  * Remember Password - Enable it

Hit save, and it should connect.

NOTE: I don't think you can connect to this from a local install of pgAdmin
think the hostname would be localhost

#### Testing DB

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

## Docker Commands

### To start the application
```
docker compose up
```

### To stop the application
```
docker compose down
```

### To rebuild everything
```
docker compose build --no-cache
```