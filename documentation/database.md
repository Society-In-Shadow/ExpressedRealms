## Database Basics

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
```

### Postgres
We use a postgres database.  On our locals, that db will be handled by the docker image for postgres.

On first start, the web api will populate the db and fill it in with sample data.

Connection details can be found in the docker-compose.yaml file.

The database will be persistent across the docker images build / rebuild.

#### Population Scripts
These are stored outside of the repo, and you will need to ask about getting access to them.

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

On the popup, there are two tabs you need to be concerned about to get this up and running: General and Connections


##### General Tab
For the general tab, only value you really need to be concerned about is the name field, the rest of them you don't need
to set

| Field    | Value             |
|----------|-------------------|
| Name     | Expressed Realms  |


##### Connection Tab
NOTE: Env File variables are case sensitive

| Field                     | Value                              |
|---------------------------|------------------------------------|
| Hostname/Address          | expressed-realms-db                |
| Port                      | 5432                               |
| Maintenance Database      | From the env file: expressedRealms |
| UserName                  | From the env file: DB_User         |
| Kerberos authentication?  | Don't Enable                       |
| Password                  | From the env file: DB_Password     |
| Save Password             | Enable it                          |
| Role                      | Leave Blank                        |
| Service                   | Leave Blank                        |


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