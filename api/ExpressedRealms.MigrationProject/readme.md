The purpose of this project is to allow for easy creation of migrations without having to rely on the main web api

# Expressed Realms DB

## To create migration

Go to the root of project, (folder above this one), and type the following:
```shell
dotnet ef migrations add <migration name> --project ExpressedRealms.DB --startup-project ExpressedRealms.MigrationProject
```


You might run into permission issues, especially if you use docker.  So what you need to do is delete the obj and bin
folders for both the server project and the db project.

## To Update the DB

To automatically apply the update, just run docker compose run in the root folder.  That will automatically push the
update.

If you have a separate instance up and running, you can use the following command:
```shell
dotnet ef database update --verbose --project ExpressedRealms.DB --startup-project ExpressedRealms.MigrationProject
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
