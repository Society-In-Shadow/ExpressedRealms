# Expressed Realms

[Expressed Realms is awesome!](https://www.youtube.com/watch?v=9UJ1syXaNoQ&t=13s)

## Quick Start

### Getting the Codebase

You want to download [Docker Desktop](https://desktop.github.com/)

This installer will automatically install itself when you run it.
Once it installs, it will prompt you to login.

Depending on your status of your GitHub account, you might need to enable 2FA

Society-In-Shadow should show up as an organization, Expressed Realms is the name of the repository.

Click on it, click clone, make sure to note where it's be put in the system (2nd text field)

If it doesn't show up, let Cameron know, he'll get you access.

### Docker Desktop

Docker is required to run this application locally.

Download and install [Docker Desktop](https://www.docker.com/products/docker-desktop/).
Follow their instructions to get docker up and running : [Install Windows](https://docs.docker.com/desktop/install/windows-install/#install-docker-desktop-on-windows)

Once you have their hello world example up and running, you should be good to go.

### Setup Certificates

#### Fedora

Follow these steps for the most part, except for part 2

[Stack Overflow Steps](https://stackoverflow.com/a/59702094)

Part 2, you need to do this instead
sudo trust anchor --store localhost.crt

after which, this should return OK
openssl verify localhost.crt

Step 3 does work, but won't work with production stuff

next step is to move the pfx file into ~/.aspnet/https and chmod 777 the entire directory and file

#### Windows

##### Chocolatey and mkcert
To Download and install chocolatey, run this in an admin powershell
```shell
Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
```
Now run
```shell
mkdir -p $env:USERPROFILE\.aspnet\https\
cd $env:USERPROFILE\.aspnet\https\
```

That should get you to that folder in your user profile.

Now run these commands

```shell
mkcert -key-file key.pem -cert-file cert.pem localhost
mkcert -pkcs12 localhost
```

Then you need to rename the localhost file

```shell
mv .\localhost.p12 localhost.pfx
```

That will at least get the API working to the point where you can run it without it throwing errors.  When you try loading
the Web API, it will say that the certificate is not secure, you can ignore that for now.

### Configure DB Stuff

With all of that in mind, we need to go to the repo you downloaded earlier.  In the root folder (Same folder as this
readme), you need to create a ".env" file.  In said file, add this information.

Side Note: Avoid spaces on the right hand side of the values

```ini
# pgAdmin is the db management tool. 
# These values are your login credentials
PGADMIN_EMAIL=
PGADMIN_PASSWORD=

# This is the db name, plus the user and 
# password you need to connect to it
DB_NAME=expressedRealms
DB_USER=
DB_PASSWORD=

# This is predefined with the mkcert command.  Keep the same
CERTIFICATE_PASSWORD=changeit
```


### Run Society in Shadows

Once you get docker up and running, and get the environment file in place, you should be good to go to start the website.

What you want to do is open up the root of the project (the same directory as this readme) in powershell, and run the
following command.

```shell
docker compose up
```

It will start to do a lot of things.  If this is the first run, it will take some time to download stuff.

Once everything has been downloaded, it should start db followed by the vue app.  Once the DB is up and running, it will
start the web api, then the pgAdmin.

Once the messages cool down, you can visit links below.

NOTE: The front end might take a bit to load, as first load takes a bit.

### Important Links

* [Front End / Web App](https://localhost:5173/)
* [Back End / Web API / Swagger](https://localhost:5001/swagger/index.html)
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)
* [SendGrid Testing](http://localhost:7000)

## Database Basics

### Postgres
We use a postgres database.  On our locals, that db will be handled by the docker image for postgres.

On first start, the web api will populate the db and fill it in with sample data.

Connection details can be found in the docker-compose.yaml file.

The database will be persistent across the docker images build / rebuild.  

#### Reset DB

If you want to nuke the db, run this command first:

```shell
docker volume ls
```

With that list, there should be a db_data volume

Once you find that, copy it and run this command.

NOTE: this is case sensitive

```shell
docker volume rm nameOfVolume
```
That name should be fairly consistent going forward, so you won't need to run the first step all the time.

### pgAdmin
pgAdmin is a postgres database management tool.  For Expressed Realms, it runs in a docker container.

When the app is started for the first time, it will create a new directory

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
  * Maintenance Database - From the env file: DB_Name (Case Sensitive)
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