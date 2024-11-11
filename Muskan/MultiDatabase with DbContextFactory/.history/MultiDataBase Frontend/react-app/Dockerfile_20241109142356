# Use Node 20 for the build stage
FROM node:20
WORKDIR /react-app

# Copy files into the container
COPY package.json /react-app/package.json
COPY public /react-app/public
COPY src /react-app/src

# Install dependencies
RUN npm install

# Copy all source files
COPY . ./ 

# Build the React app
RUN npm run build

# Expose port 3000 for the React app
EXPOSE 3000

# Start the React app
CMD ["npm", "start"]
