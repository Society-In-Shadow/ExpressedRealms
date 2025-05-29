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