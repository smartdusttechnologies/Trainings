# FROM node:20

# WORKDIR /reacts
# COPY "./MultiDataBase-Frontend/react-app/package.json" ./reacts/package.json
# COPY "./MultiDataBase-Frontend/react-app/package-lock.json" ./reacts/package-lock.json
# RUN npm install

# EXPOSE 3000


# CMD ["npm", "run", "start"]
FROM node:20


WORKDIR /reacts

COPY package.json .
COPY package-lock.json .


RUN npm install

COPY . .


EXPOSE 3000

CMD ["npm", "run", "start"]

