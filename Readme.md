# Expressed Realms

## Welcome to Society In Shadows!

This github repository is the home of the Society In Shadows project.

There are two main purposes of this project

- Provide a platform to store all the lore, expressions, game mechanics, and other information about the Society
- Provide a platform for creating and maintaining character sheets for the residents of the Society

# Where to go from here

## Current Progress and Goals
An up-to-date list of all broad objectives can be found in the [Milestones](https://github.com/Society-In-Shadow/ExpressedRealms/milestones) 
section of the project.  You can also take a look at the Issues tab as well to see more fine grained tasks.

## Non Developers
Please go to the [main website](https://society-in-shadows.com/) to get started.

Or join our discord group here [Discord](https://discord.gg/6yJDurTdJa)

## Developers
Overall architecture [here](/documentation/architecture.md)

## Quick Start

To get this up and running, download the repo
 
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

With all that out of the way, run the following command in the root of the repo (Same folder as this readme)

```shell
docker compose up
```
That should get the site up and running for the most part, minus the population data.

To get the full experience, feel free to create a user at [https://society-in-shadows.org](https://society-in-shadows.com/)

## Local Links
Links to various places locally can be found here:
* [Front End / Web App](https://localhost/)
* [Back End / Web API / Swagger](https://localhost:5001/swagger/index.html)
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)
* [Feature Flags](http://localhost:8050)
* [Email Testing](http://localhost:8025)

## Data Population Scripts
These are stored separate if you would like access, ask about it in the discord group
