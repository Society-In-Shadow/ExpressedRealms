# Linux Setup Instructions

## Inotify Limitations
Running into Inotify limitations is something that will probably happen, especially with podman running a bunch of different
docker containers.

To address this, you need to increase the number of allowed instances.

By default on ubuntu, the values that are set are the following (or at least was on my local)

```ts
fs.inotify.max_queued_events = 16384
fs.inotify.max_user_instances = 128
fs.inotify.max_user_watches = 65536
```

To increase them to Vite's recommended defaults, run the following commands, then restart your computer.
```
sudo sysctl fs.inotify.max_queued_events=16384
sudo sysctl fs.inotify.max_user_instances=8192
sudo sysctl fs.inotify.max_user_watches=524288
```

For some background here, that number was set as a default a long time ago, and hasn't been updated for quite a while.
Typical workloads don't need much, but with developers, it can be a problem.

More info can be found in their [troubleshooting guide regarding the dev server]( https://www.kernel.org/doc/html/latest/filesystems/inotify.html)

## Debugging using Android Phone
If you are using ubuntu, you need to install the following package

```shell
sudo apt install adb
```

This will allow chrome/desktop to ask for debugging permissions on your phone.

You will need to start it when you want to debug.

```shell
adb start-server
```

Once it's been connected and you accept the connection prompt, it will automatically connect if it's running.