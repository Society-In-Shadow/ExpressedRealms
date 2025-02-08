# build stage
FROM node:20 as build-stage

# Ideally this should be set in the github action, but it's not picking up the value
# so setting it to production here, and overriding it locally.  See #147
ARG MODE=production

# Declare build argument
ARG API_SERVER_LOCATION
ENV API_SERVER_LOCATION=$API_SERVER_LOCATION

#ARG COOKIE_DOMAIN_NAME
#ENV COOKIE_DOMAIN_NAME=$COOKIE_DOMAIN_NAME

WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build-only -- --mode $MODE

# production stage
FROM nginx:stable-alpine as production-stage

COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY nginx.conf.template /etc/nginx/nginx.conf.template

RUN envsubst '${API_SERVER_LOCATION}' < /etc/nginx/nginx.conf.template > /etc/nginx/nginx.conf

RUN cat /etc/nginx/nginx.conf

RUN chown -R nginx:nginx /var/cache/nginx && \
        chown -R nginx:nginx /var/log/nginx && \
        chown -R nginx:nginx /etc/nginx/conf.d
RUN touch /var/run/nginx.pid && \
        chown -R nginx:nginx /var/run/nginx.pid

HEALTHCHECK --interval=30s --timeout=3s CMD curl --fail http://localhost:3000/ || exit 1

# Optional: Add custom Nginx configuration
EXPOSE 80
USER nginx
CMD ["nginx", "-g", "daemon off;"]
