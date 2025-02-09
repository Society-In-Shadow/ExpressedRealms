# build stage
FROM node:20 as build-stage

# Ideally this should be set in the github action, but it's not picking up the value
# so setting it to production here, and overriding it locally.  See #147
ARG MODE=production

COPY .env.production /usr/share/nginx/html

WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build-only -- --mode $MODE

# production stage
FROM nginx:stable-alpine as production-stage

COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

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
