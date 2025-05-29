## Emails

The app uses sendgrid to send emails, locally, it doesn't actually hit the API.  Instead it hits a local docker image that
is emulating sendgrid.

As a result, messages are not actually sent, they are being stored in that docker image.

The emails can be grabbed from here:

[Email Server](http://localhost:7000)
