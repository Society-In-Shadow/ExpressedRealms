FROM node:current-alpine3.20
WORKDIR /app
COPY . .
RUN npm install

USER node
CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"]