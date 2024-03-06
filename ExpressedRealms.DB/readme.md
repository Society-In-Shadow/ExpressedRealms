# Expressed Realms DB

## To create migration

Go to the root of project, (folder above this one), and type the following:
```shell
dotnet ef migrations add <migration name> --project ExpressedRealms.DB --startup-project ExpressedRealms.Server
```


You might run into permission issues, especially if you use docker.  So what you need to do is delete the obj and bin 
folders fro both the server project and the db project.

## To Update the DB

To automatically apply the update, just run docker compose run in the root folder.  That will automatically push the
update.

If you have a separate instance up and running, you can use the following command:
```shell
dotnet ef database update --verbose --project ExpressedRealms.DB --startup-project ExpressedRealms.Server
```

### Creating new DB Objects

So we effectively have configuration classes that we can use to build objects out.  Included in this is the default data
the application has.  See the CharacterConfiguration class for an example.

* [Seeding](https://code-maze.com/migrations-and-seed-data-efcore/)
* [Type Configuration](https://stackoverflow.com/questions/46978332/use-ientitytypeconfiguration-with-a-base-entity)

