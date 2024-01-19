FROM node:16-alpine

# Security related, prevent docker from running as root user
# Create a system group named "user" with the -r flag
RUN groupadd -r user
RUN useradd -r -g user user
RUN chown -R user:user /usr/local/apps/myapp
USER user

WORKDIR /usr/local/apps/myapp

COPY package.json ./
RUN npm install && npm cache clean --force
ENV PATH=/usr/local/myapp/node_modules/.bin:$PATH

WORKDIR /usr/local/apps/myapp/dev
COPY tsconfig.json ./

COPY src ./src

EXPOSE 80

CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"]