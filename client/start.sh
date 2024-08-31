#!/bin/ash

# Choose config file based on environment variable
if [ "$APP_ENV" = "Production" ]; then
    cp -f /etc/nginx/nginx.prod.conf /etc/nginx/nginx.conf
else
    cp -f /etc/nginx/nginx.dev.conf /etc/nginx/nginx.conf
fi

# Start Nginx
nginx -g "daemon off;"