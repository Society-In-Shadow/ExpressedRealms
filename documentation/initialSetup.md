
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