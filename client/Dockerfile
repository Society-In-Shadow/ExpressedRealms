# build stage
FROM node:18 as build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build-only

# production stage
FROM nginx:stable-alpine as production-stage

COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY nginx.dev.conf /etc/nginx/nginx.dev.conf
COPY nginx.prod.conf /etc/nginx/nginx.prod.conf

CMD rm /etc/nginx/nginx.conf
COPY start.sh /usr/local/bin/start.sh
RUN chown -R nginx:nginx /usr/local/bin/start.sh
RUN chmod +x /usr/local/bin/start.sh

RUN chown -R nginx:nginx /etc/nginx/nginx.dev.conf
RUN chown -R nginx:nginx /etc/nginx/nginx.prod.conf

RUN chown -R nginx:nginx /var/cache/nginx && \
        chown -R nginx:nginx /var/log/nginx && \
        chown -R nginx:nginx /etc/nginx/conf.d
RUN touch /var/run/nginx.pid && \
        chown -R nginx:nginx /var/run/nginx.pid

HEALTHCHECK --interval=30s --timeout=3s CMD curl --fail http://localhost:3000/ || exit 1

# Optional: Add custom Nginx configuration
EXPOSE 80
USER nginx
CMD ["/usr/local/bin/start.sh"]
