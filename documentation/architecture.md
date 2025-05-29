# High Level Overview

## Frontend
* [Front End / Web App](https://localhost:5173/)


## Backend / Web API
* [Back End / Web API / Swagger](https://localhost:5001/swagger/index.html)


## Database
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)

## Testing

### Unit Tests
As of writing, the only unit tests implemented are vitests.
This is mainly to test the front end pinia stores and custom logic.

The github actions will run them on every push to a PR, and they must all be passing to merge

You can find more information here:
*

### Component Tests
Cypress is being used for component testing.  When writing these, we are looking for complete coverage of the functional
part of the page.  This means that we are not testing the styling or the layout.  This means testing to make sure that
all fields are filled out, error messages are being displayed, data is being saved, actions are correctly connected up
and so on.

More information can be found here:
* Cypress


### End to End Tests


## Feature Flags
* [Feature Flags](http://localhost:8050)

## Notifications
For this application, there isn't much in terms of notifications.  As of right now we are using sendgrid to send user registration emails.
* [SendGrid Testing](http://localhost:7000)