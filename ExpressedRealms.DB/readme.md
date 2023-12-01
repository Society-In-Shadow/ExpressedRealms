
#To create migration:

Go to the root of the folder and run this command

dotnet ef migrations add <migration name> --project ExpressedRealms.DB --startup-project ExpressedRealms.Server

#To Run
Move to server project and run this
dotnet ef database update 