# Expressed Realms DB

## To create migration

Go to the root of the folder and run this command

dotnet ef migrations add <migration name> --project ExpressedRealms.DB --startup-project ExpressedRealms.Server

## To Update the DB
dotnet ef database update --verbose --project ExpressedRealms.DB --startup-project ExpressedRealms.Server

### Creating new DB Objects

So we effectively have configuration classes that we can use to build objects out.  Included in this is the default data
the application has.  See the CharacterConfiguration class for an example.

* [Seeding](https://code-maze.com/migrations-and-seed-data-efcore/)
* [Type Configuration](https://stackoverflow.com/questions/46978332/use-ientitytypeconfiguration-with-a-base-entity)

