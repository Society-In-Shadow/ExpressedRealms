# Expressed Realms

## Welcome to Society In Shadows!

This github repository is the home of the Society In Shadows project.

There are two main goals for this project

- Provide a platform to store all the lore, expressions, game mechanics, and other information about the Society
- Provide a platform for creating and maintaining character sheets for the residents of the Society

# Current Progress and Goals
An up-to-date list of all broad goals can be found in the [Milestones](https://github.com/Society-In-Shadow/ExpressedRealms/milestones) 
section of the project.  You can also take a look at the Issues tab as well to see more fine grained tasks.

# Non Developers
Please go to the [https://societyinshadows.org](https://societyinshadows.com/) to get started.

Or join our discord group here [Discord](https://discord.gg/NSv3GxSAj7)

# Developers
In addition to the quick start below, the high level architecture and technologies can be found [here](/documentation/architecture.md)

# Quick Start

To get this up and running, first, you need to download the repo
 
## Setup the .env File
Next up, add the following snippet as an ".env" file to the root of the repo (same folder this readme is in)

Fill in the blanks below, each email / user / password should be filled in

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
```

## Create SSL Certs
Next, you need to setup SSL Certs

* Windows users can follow the instructions [here](/documentation/localSSLCerts.md)
* Fedora users can follow the instructions [here](/documentation/fedoraSetup.md)

## Start the Website
Once you get docker up and running, add the .env file, and get the SSL certs setup, you should be good to go to 
start the website.

What you want to do is open up the root of the project (the same directory as this readme) with command line or terminal,
and run the following command.

```shell
docker compose up
```

At this point, it will start doing a lot of things all together.  If this is the first time running this, it will take
time to download all the images, then get things started.

Once everything has been downloaded, it should start db followed by the vue app.  Once the DB is up and running, it will
start the web api, then the pgAdmin.

Once the messages cool down, you can visit links below.

## Local Links
Links to various places locally can be found here:
* [Front End / Web App](https://localhost/)
* [Back End / Web API / Swagger](https://localhost:5001/swagger/index.html)
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)
* [Feature Flags](http://localhost:8050)
* [Email Testing](http://localhost:8025)

## Data Population Scripts
These are stored separate if you would like access, ask about it in the discord group

To get the full experience with all the data, feel free to create a user at [https://societyinshadows.org](https://society-in-shadows.com/)
