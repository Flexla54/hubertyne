# Build stage
FROM node:18.14 AS build

WORKDIR /app

RUN npm i -g @angular/cli

COPY src/ /app/src/
COPY angular.json /app/
COPY package.json /app/
COPY package-lock.json /app/
COPY tsconfig.app.json /app/
COPY tsconfig.json /app/

RUN npm i
RUN ng build

# Run stage
FROM nginx:alpine as run

COPY --from=build /app/dist/temp-spa/ /app/
COPY nginx.conf /etc/nginx/nginx.conf
