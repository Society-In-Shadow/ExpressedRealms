# High Level Overview

## Frontend
The frontend is built using Vue 3 and Vite.  It primarily uses PrimeVue and Boostrap for styling and layout.

Locally, it can be found at this address
* [Front End / Web App](https://localhost/)



## Backend / Web API
Backend is using .Net 8, primarily using the repository pattern to store business logic.  

More information on the repository standard we use can be found [here](/repositoryStandards.md)

The API is built using Minimal APIs, following the restful api pattern.  The swagger documentation is being used to document the API.

Fluent Validations and Fluent Results are used extensively to validate and pass around the data in the backend.

Locally, swagger can be found at this address
* [Back End / Web API / Swagger](https://localhost:5001/swagger/index.html)

## Database
We are using Postgres for the database, and using PgAdmin locally to manage it.

The codebase interacts with it via Entity Framework Core.

The database for the most part is a dumb one, all business logic should be handled in the repositories.

Locally, PgAdmin can be found at this address
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)

### More Information
* More information on how we use it can be found in the [Database](database.md) documentation
* More information on how Migrations work can be found in the [Migrations](migrations.md) documentation
* More information on how we use EF Core can be found in the [EF Core Practices](efCorePractices.md) documentation

## Auditing
In here we have user logs, that keep track of admin related actions and anything to do with modifying none user specific 
data.

We are using a library called Audit.Net, that integrates with EF Core.

At it's core, it keeps track of changes on the column level for all tables.

I have a wrapper around that to make it easier to use, and make sure that it outputs a user activity log.

More information on how we use the Audit Framework can be found in the [EF Core Practices](efCorePractices.md) documentation

## Testing

### Unit Tests
As of writing, the only unit tests implemented are vitests.
This is mainly to test the front end pinia stores and custom logic.

The github actions will run them on every push to a PR, and they must all be passing to merge

### Component Tests
Cypress is being used for component testing.  When writing these, we are looking for complete coverage of the functional
part of the page.  This means that we are not testing the styling or the layout.  This means testing to make sure that
all fields are filled out, error messages are being displayed, data is being saved, actions are correctly connected up
and so on.

See [Component Testing](componentTesting.md) for more information about how we use it here

### End to End Tests
End to end tests are also using Cypress.  As of writing, they do some basic checks, but not really maintained as much.

See [Cypress](cypress.md) for more information about how we use it here

## Feature Flags
This one is the newer kid on the block.  We are using a product called FlipT.

In addition to hosting this locally at the address down below, we are also hosting it out in production.

* [Feature Flags](http://localhost:8050)

## Notifications
For this application, there isn't much in terms of notifications.  As of right now, it just user registrations

Locally, we are using a product called Mailpit to view the emails that are being sent
* [Mailpit](http://localhost:8050)

And out on prod we are using a product called Postmark

## CI/CD
We are using github actions for CI/CD.  The github actions are located in the .github/workflows folder.

# Hosting

## Docker Compose (Local)
Locally, everything is running via docker containers.  All services should be configured through that.

See more information in the [Docker](docker.md) documentation

## Azure
For our production environment, we rely on Azure.