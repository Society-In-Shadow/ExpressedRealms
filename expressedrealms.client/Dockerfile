FROM node:16-alpine

# Create a non-root user
RUN addgroup -g 1001 rootless && \
    adduser -D -u 1001 -G rootless rootless

WORKDIR /usr/local/apps/myapp
COPY package.json ./
RUN chown -R rootless:rootless /usr/local/apps/myapp

USER rootless

# Install dependencies
RUN npm install && npm cache clean --force
ENV PATH=/usr/local/apps/myapp/node_modules/.bin:$PATH

# Switch back to the root user to copy source code
USER root

WORKDIR /usr/local/apps/myapp/dev
COPY tsconfig.json ./
COPY src ./src

# Switch to the non-root user
USER rootless

HEALTHCHECK --interval=15s --timeout=60s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider https://0.0.0.0:5173/login || exit 1

EXPOSE 80

CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"]