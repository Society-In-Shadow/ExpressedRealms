# syntax=docker/dockerfile:1
# https://github.com/BretFisher/nodejs-rocks-in-docker/blob/main/dockerfiles/1.Dockerfile
###
## Example: The most basic, CORRECT, Dockerfile for Node.js
###

# alwyas use slim and the lastest debian distro offered
FROM node:22.4-bookworm-slim@sha256:ee76feb064dbe3579085bc2517cb54ecf64b083db8f6f80341cfe4a4770d1415

RUN apt-get update && apt-get install -y curl=7.88.1 --no-install-recommends  && rm -rf /var/lib/apt/lists/*

EXPOSE 443

# add user first, then set WORKDIR to set permissions
USER node

WORKDIR /app

# copy in with correct permissions. Using * prevents errors if file is missing
COPY --chown=node:node package*.json ./

# use ci to only install packages from lock files
RUN npm ci --omit=dev && npm cache clean --force

# copy files with correct permissions
COPY --chown=node:node . .

# Health check using curl
HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl -fs https://localhost --insecure || exit 1

# change command to run node directly
CMD ["node", "./bin/www"]