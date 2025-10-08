# pgAdmin4

pg admin is sql's verison of SQL Manager, it allows one to configure and update postgres databases

## Installing pdAdmin4

Then in pgAdmin 4, you want to open up preferences, go to Paths > Binary Paths, and on the bottom half, use this file path

/usr/lib/postgresql/16/bin

[Install Instructions](https://www.pgadmin.org/download/pgadmin-4-apt/)

```bash
sudo apt-get install curl
curl -fsS https://www.pgadmin.org/static/packages_pgadmin_org.pub | sudo gpg --dearmor -o /usr/share/keyrings/packages-pgadmin-org.gpg
sudo sh -c 'echo "deb [signed-by=/usr/share/keyrings/packages-pgadmin-org.gpg] https://ftp.postgresql.org/pub/pgadmin/pgadmin4/apt/$(lsb_release -cs) pgadmin4 main" > /etc/apt/sources.list.d/pgadmin4.list && apt update'
sudo apt install pgadmin4
```

## Enable database tools locally

Azure uses 16.9 as of writing, so you want to run these two commands
this will allow you to use multiple different versions of postgres locally

This is for Ubuntu

```bash
sudo apt install -y postgresql-common
sudo /usr/share/postgresql-common/pgdg/apt.postgresql.org.sh
```

Then install this

```bash
sudo apt-get install postgresql-client-16
```

## Backup / Restoring

Only thing you need to select is Do Not Save Owner and Do not Save Privileges, make sure to give the file .backup in pgAdmin.
To restore, select the database and restore the .backup file
