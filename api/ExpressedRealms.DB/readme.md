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

### Creating new DB Objects

So we effectively have configuration classes that we can use to build objects out.  Included in this is the default data
the application has.  See the CharacterConfiguration class for an example.

* [Seeding](https://code-maze.com/migrations-and-seed-data-efcore/)
* [Type Configuration](https://stackoverflow.com/questions/46978332/use-ientitytypeconfiguration-with-a-base-entity)

# Using Audit.Net
You need a few things to work with Audit.net
 - You need to include [AuditInclude] for the class that needs audits
 - You need to copy over ExpressionAuditTrail + config files to the corresponding class you wish to track
   - Make sure you include all primary keys with the new class
 - You need to update SetupDatabaseAudit.cs with the new mapping, follow existing examples.
   - Main thing to look out for is mapping the primary keys for the table back 

## Notes
 - On insert, it will ignore any the id column, plus the DeletedAt and IsDeleted columns
 - On Edit it will not ignore that, as we want to keep track of when something was removed along with all the other records