# Setup Certificates (Windows)

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