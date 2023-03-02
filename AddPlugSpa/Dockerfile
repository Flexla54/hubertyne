# Build stage
FROM node:18.14 AS build

WORKDIR /app

RUN npm i -g parcel

COPY src/ /app/src/

RUN parcel build ./src/addPlug.html --dist-dir /app/dist/add-plug-spa

# Run stage
FROM nginx:alpine as run

COPY --from=build /app/dist/add-plug-spa/ /app/
COPY nginx.conf /etc/nginx/nginx.conf
