# Database Basics

## Postgres

We use a postgres database.  On our locals, that db will be handled by the docker image for postgres.

On first start, the web api will create an empty database, that you will then need to restore a sanitized db backup to.

The database will be persistent across the docker images build / rebuild.

## Reset DB

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

## pgAdmin

pgAdmin is a postgres database management tool.  Expectation is that you will run this as an native app on your machine.

### Connect to DB

When you visit it, it will prompt you for a username and password

To login, take a look at that ".env" file you created, it's the credentials you put there.

Once you get in, you need click add server.

On the popup, there are two tabs you need to be concerned about to get this up and running: General and Connections

#### General Tab

For the general tab, only value you really need to be concerned about is the name field, the rest of them you don't need
to set

| Field    | Value             |
|----------|-------------------|
| Name     | Expressed Realms  |

#### Connection Tab

NOTE: Env File variables are case sensitive

| Field                     | Value       |
|---------------------------|-------------|
| Hostname/Address          | localhost   |
| Port                      | 5432        |
| Maintenance Database      | postgres    |
| UserName                  | dev_user    |
| Kerberos authentication?  | Don't Enable |
| Password                  | dev_password |
| Save Password             | Enable it   |
| Role                      | Leave Blank |
| Service                   | Leave Blank |

Hit save, and it should connect.

### Testing DB

Once ran, there should be a database called expressedRealms.  This is what you will restore a backup to.


## Backup / Restoring

Only thing you need to select is Do Not Save Owner and Do not Save Privileges, make sure to give the file .backup in pgAdmin.
To restore, select the database and restore the .backup file

In addition, the postgres instance in Azure gets updated automatically, so there might be times where you cannot restore
locally, until you upgrade the db instance in the compose file.  

The database version displayed in the portal pgAdmin with select version() isn't indicative of the versions you need, 
it's the underlaying runtime that is the thing that needs to match

Ultimately, if the restore isn't working, it's probably because the runtime needs to be updated in compose.