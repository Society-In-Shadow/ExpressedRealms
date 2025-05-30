# Expressed Realms DB

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