
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