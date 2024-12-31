# Expressed Realms

[Expressed Realms is awesome!](https://www.youtube.com/watch?v=9UJ1syXaNoQ&t=13s)

## Quick Start

### Getting the Codebase

You want to download [Github Desktop](https://desktop.github.com/)

This installer will automatically install itself when you run it.
Once it installs, it will prompt you to login.

Depending on your status of your GitHub account, you might need to enable 2FA

Society-In-Shadow should show up as an organization, Expressed Realms is the name of the repository.

Click on it, click clone, make sure to note where it's be put in the system (2nd text field)

If it doesn't show up, let Cameron know, he'll get you access.

### Docker Desktop

Docker is required to run this application locally.

Download and install [Docker Desktop](https://www.docker.com/products/docker-desktop/).
Follow their instructions to get docker up and running : [Install Windows](https://docs.docker.com/desktop/install/windows-install/#install-docker-desktop-on-windows)

Once you have their hello world example up and running, you should be good to go.

### Setup Certificates (Windows)

Use an admin powershell for the following commands.

To Download and install chocolatey:
```shell
Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
```

Now run the following to get mkcert locally
```shell
choco install mkcert
```

With that, you need to make sure that you have the appropriate folders, and start executing commands from them.
```shell
mkdir -p $env:USERPROFILE\.aspnet\https\
cd $env:USERPROFILE\.aspnet\https\
```

That should get you to that folder in your user profile.  If it says the folder already exists, just use the CD command.

Now run these commands

```shell
mkcert -key-file key.pem -cert-file cert.pem localhost
```

When you visit the sites, both Chrome and firefox will consider the certificates to be invalid, as it doesn't trust
local certificates.  If you click through it, it should let you in though.

### Configure DB Stuff

With all of that in mind, we need to go to the repo you downloaded earlier.  In the root folder (Same folder as this
readme), you need to create a ".env" file.  In said file, add this information.

Side Note: Avoid spaces on the right hand side of the values

```ini
# pgAdmin is the db management tool. 
# These values are your login credentials
PGADMIN_EMAIL=
PGADMIN_PASSWORD=

# This is the db name, plus the user and 
# password you need to connect to it
DB_NAME=expressedRealms
DB_USER=
DB_PASSWORD=

# This is predefined with the mkcert command.
# DO NOT modify this
CERTIFICATE_PASSWORD=changeit

# Ask about this key
POSTMARK_API_KEY=

# Global From Email
NO_REPLY_EMAIL=admin@societyinshadows.org

# Email address the admin test email sends to
TEST_EMAIL_ADDRESS=YourEmailAddressHere
```

### Note About Vite and Windows

Hot reloading using Vite won't work with docker compose, due to fundamental issue with how WSL works.

See:
[Broken File Watcher](https://github.com/microsoft/WSL/issues/4739)

Fundamentally, since the docker runs it's containers on WSL, when you update something on windows, that update isn't being
translated into the linux version of hey a file updated process,  thus not vite to pick up the changes.

There are some workarounds, the only one that I could get working is below.

To enable it, though at a cost of CPU run time, add the following to the server portion of vite config
``` javascript
watch: {
  usePolling: true
}
```

### Run Expressed Realms

Once you get docker up and running, and get the environment file in place, you should be good to go to start the website.

What you want to do is open up the root of the project (the same directory as this readme) in powershell, and run the
following command.

```shell
docker compose up
```

It will start to do a lot of things.  If this is the first run, it will take some time to download stuff.

Once everything has been downloaded, it should start db followed by the vue app.  Once the DB is up and running, it will
start the web api, then the pgAdmin.

Once the messages cool down, you can visit links below.

NOTE: The front end might take a bit to load, as first load takes a bit.

### Important Links

* [Front End / Web App](https://localhost:5173/)
* [Back End / Web API / Swagger](https://localhost:5001/swagger/index.html)
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)
* [SendGrid Testing](http://localhost:7000)

## Database Basics

### Postgres
We use a postgres database.  On our locals, that db will be handled by the docker image for postgres.

On first start, the web api will populate the db and fill it in with sample data.

Connection details can be found in the docker-compose.yaml file.

The database will be persistent across the docker images build / rebuild.

#### Reset DB

If you want to nuke the db, run this command first:

```shell
docker volume ls
```

With that list, there should be a db_data volume

Once you find that, copy it and run this command.

NOTE: this is case sensitive

```shell
docker volume rm nameOfVolume
```
That name should be fairly consistent going forward, so you won't need to run the first step all the time.

### pgAdmin
pgAdmin is a postgres database management tool.  For Expressed Realms, it runs in a docker container.

When the app is started for the first time, it will create a new directory

You can access here:
* [DB Management / pgAdmin](http://localhost:8888/login?next=%2Fbrowser%2F)


#### Connect to DB

When you visit it, it will prompt you for a username and password

To login, take a look at that ".env" file you created, it's the credentials you put there.

Once you get in, you need click add server.

On the popup, there are two tabs you need to be concerned about to get this up and running: General and Connections


##### General Tab
For the general tab, only value you really need to be concerned about is the name field, the rest of them you don't need
to set

| Field    | Value             |
|----------|-------------------|
| Name     | Expressed Realms  |


##### Connection Tab
NOTE: Env File variables are case sensitive

| Field                     | Value                              |
|---------------------------|------------------------------------|
| Hostname/Address          | expressed-realms-db                |
| Port                      | 5432                               |
| Maintenance Database      | From the env file: expressedRealms |
| UserName                  | From the env file: DB_User         |
| Kerberos authentication?  | Don't Enable                       |
| Password                  | From the env file: DB_Password     |
| Save Password             | Enable it                          |
| Role                      | Leave Blank                        |
| Service                   | Leave Blank                        |


Hit save, and it should connect.

NOTE: I don't think you can connect to this from a local install of pgAdmin
think the hostname would be localhost

#### Testing DB

To test: On the left hand side,
* Servers
  * Expressed Realms
    * Databases
      * ExpressedRealms
        * Schemas (2)
          * public
            * tables
              * characters

Right click on that table, and hit view / edit data.

There should be 2 characters in there, John and Jane Doe.

## Emails

The app uses sendgrid to send emails, locally, it doesn't actually hit the API.  Instead it hits a local docker image that
is emulating sendgrid.

As a result, messages are not actually sent, they are being stored in that docker image.

The emails can be grabbed from here:

[Email Server](http://localhost:7000)

## Docker Commands

### To start the application
```shell
docker compose up
```

### To stop the application
```shell
docker compose down
```

### To rebuild everything
```shell
docker compose build --no-cache
```

## Fedora

### Setup Cert

### Install mkcert and supporting tools

```shell
curl -JLO "https://dl.filippo.io/mkcert/latest?for=linux/amd64"
chmod +x mkcert-v*-linux-amd64
sudo cp mkcert-v*-linux-amd64 /usr/local/bin/mkcert
```

Install nss-tools to allow the CA to be used in firefox and chrome
```shell
sudo dnf install nss-tools
```

### Create the Certs
```shell
mkcert -install

mkdir ~/.aspnet/https -p
cd ~/.aspnet/https

mkcert -cert-file cert.pem -key-file key.pem localhost
```

Since you use linux, you need to add this to your env file.  UserProfile isn't predefined on linux machines like it is
on windows
```ini
# If you use linux, set this to your home directory
USERPROFILE="/home/<username>"
```

### Issues

#### Can't Connect Docker Deamon
If you get this:
permission denied while trying to connect to the Docker daemon socket at unix:///var/run/docker.sock:

[Do this](https://stackoverflow.com/questions/47854463/docker-got-permission-denied-while-trying-to-connect-to-the-docker-daemon-socke)
```shell
sudo usermod -a -G docker <username>
```
After that log off and back on

#### Permission Issue With Certificate
You might need to chmod 777 the entire directory and files if you run into permission issues while running Docker

There are some other issues you might run into, the last time I did, I had to add the :z option to the aspnet folder in
the docker compose file.

SELinux was blocking access to the file from the container.  From what I gather, it's a kernel level module, that
by default prevents containers from access files.  It's basically wanting to enforce least privileged access.

#### Docker Compose: bind: address already in use
This can happen if you have cypress open, and you try start / stop the docker compose, specifically the vue docker.  
All you need to do is close cypress and you should be good to go.

#### Access Denied to Data Protection Keys

If you have this : System.UnauthorizedAccessException: Access to the path '/home/app/.aspnet/DataProtection-Keys/ec1cc766-d010-4f5f-b997-2f7a9a8a8b68.tmp' is denied.

So, this is happening because the id's between users on your local machine isn't matching up with the ones on the docker image.

the volumes of etx/passwd and group should fix this for the most part on your local, but you will also run into issues
with the certificates due to the same issue.

## Cypress
Cypress is configured in this repo.  Both the end to end and the component testing is implemented.
In order to run it on your local you need to go to the expressedRealms.client folder, and run one of the following commands.

For more inforamtion, see [Cypress](https://docs.cypress.io/guides/overview/why-cypress)

### Interactive GUI
When you run the below command, it will pop up a GUI that will allow you to run both types of tests and watch them run.
You can only do one or the other.  The main benefit here is that you can watch them run, and debug them in place to a degree.
The main benefit of using the GUI is that it will update in realtime, as you are modifying the components.

```shell
npx cypress open
```

### End to End Testing
The following runs the end to end testing via the command line.  It does everything the GUI does, it just does it headlessly.

```shell
npx cypress run
```

### Component Testing
Similar to the above command, it does everything the GUI does, but headlessly.

```shell
npx cypress run --component
```

### Permission Issue While Running Cypress
You also might need to use the following command if you try running Cypress Component Testing and Docker at the same time.

You need to be in the expressedRealms.client folder for this to work.

```shell
sudo chown -hR <username> node_modules/
```

