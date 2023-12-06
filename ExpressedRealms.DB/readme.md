
## To create migration:

Go to the root of the folder and run this command

dotnet ef migrations add <migration name> --project ExpressedRealms.DB --startup-project ExpressedRealms.Server

## To Update the DB
dotnet ef database update --verbose --project ExpressedRealms.DB --startup-project ExpressedRealms.Server
