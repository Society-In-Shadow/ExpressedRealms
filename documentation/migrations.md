# Creating EF Core Migrations
We use a separate project to work with the migrations.  This allows us to eventually make the database updates an 
independent process, and simplifies the dependencies needed for the migration.


## To create migration

Go to the api folder, in there, there should be one of two scripts.

createMigration.bat for Window's Users
createMigration.sh for Linux users.

When you run either file, it will ask for a migration name, needs to be in camel case, and will create the migration for
you.

You might run into permission issues, especially if you use docker.  So what you need to do is delete the obj and bin
folders for both the server project and the db project.

## To Update the DB

Currently, on API start, it will automatically roll out any updates to the database, so all you need to do is run and
build the API via

```shell
docker compose build
docker compose up
```

## To Rollback the Database
You will need to update the appsettings.Development.json to include the connection string to the db
Altertantively, you can use the app secret stuff to store it on a more permanent basis, and not have it committed

once you get that, the two commands you want to use is

To list out the transactions
```shell
dotnet ef migrations list --project ExpressedRealms.DB --startup-project ExpressedRealms.MigrationProject
```

To to revert to a specified time, use this

Keep note, nameOfMigration is not the one you want to revert, it's the name of the one before the one you want to remove
```shell
dotnet ef database update <nameOfMigration> --project ExpressedRealms.DB --startup-project ExpressedRealms.MigrationProject
```

If the IP address isn't working, use this command to find it for the connection string

```shell
docker inspect   -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' expressed-realms-db
```

After that, you will need to go through and manually remove the migrations in the migration folder, plus revert the model
snapshot to a state before you applied the changes.
