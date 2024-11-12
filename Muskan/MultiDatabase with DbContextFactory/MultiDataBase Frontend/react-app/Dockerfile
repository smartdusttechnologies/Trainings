
FROM node:20 AS build
WORKDIR /reacts


COPY ./MultiDataBase Frontend/react-app/package.json ./package.json
COPY ./MultiDataBase Frontend/react-app/package-lock.json ./package-lock.json


RUN npm install


COPY ./MultiDataBase Frontend/react-app/ ./reacts


RUN npm run build


EXPOSE 3000


CMD ["npm", "run", "start"]
