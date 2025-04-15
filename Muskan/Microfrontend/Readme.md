# Microfrontend Project Name

## Overview

This project is part of a microfrontend architecture designed to create a modular and scalable web application. The microfrontends involved in this project include **Auth**, **Dashboard**, **Container**, and **Marketing (Landing Page)**, each responsible for distinct features of the application. The goal is to provide a seamless user experience by integrating these microfrontends into a host application using Webpack Module Federation.

- **Auth Microfrontend**: Handles user authentication, including login and registration.
- **Dashboard Microfrontend**: Displays the dashboard
- **Container Microfrontend**: Serves as the host application that loads and integrates the other microfrontends.
- **Marketing (Landing Page) Microfrontend**: Provides the marketing landing page for the application

### Integration:

Each microfrontend communicates with the host application and the They are dynamically loaded using Webpack Module Federation, allowing for independent development, testing, and deployment of each component.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#configuration)
- [Testing](#testing)
- [Development](#development)
- [Deployment](#deployment)
- [License](#license)

## Architecture

### General Architecture

This microfrontend setup follows a modular architecture where multiple microfrontends interact with the host application. The microfrontends can be updated independently, which helps scale the system more efficiently.

- **Host Application (Container Microfrontend)**: This is the core application that dynamically loads and integrates other microfrontends.
- **Microfrontend 1 (Auth)**: Handles user authentication processes such as login and registration.
- **Microfrontend 2 (Dashboard)**: Provides the user dashboard
- **Microfrontend 3 (Marketing/Landing Page)**: Displays marketing content,

#### Technologies Used

- **React.js**: For building the user interfaces for all the microfrontends.
- **Webpack Module Federation**: Allows microfrontends to be loaded dynamically and share common dependencies, reducing redundancy.
- **Auth0** : For authentication Service

#### Communication Flow

- The **host application** (Container Microfrontend) uses Webpack Module Federation to load and display the other microfrontends.
- **Auth Microfrontend** handles the login state and shares authentication tokens with other microfrontends.
- **Dashboard Microfrontend** and **Marketing Microfrontend**

## Features

- **Auth Microfrontend**:
  - User login and registration.
- **Dashboard Microfrontend**:
  - Simple user dahboard
- **Container Microfrontend**:
  - Manages the overall layout and routing of the application.
  - Integrates the individual microfrontends into a seamless user experience.
- **Marketing (Landing Page) Microfrontend**:
- Simple Landing page with pricing section

## Installation

### Prerequisites

Ensure that you have the following installed:

- **Node.js** (v16 or higher)
- **npm**

### Steps to Install

1. **Clone the repository**:

   ```bash
   git clone https://github.com/smartdusttechnologies/Trainings.git
   ```

2. **Go to the project directory**

```sh
   cd  Muskan/Microfrontend
```

3.  **Open the terminal**  
    Go to different microfrontend
    1. **Go to Authetication application**

```sh
cd auth
```

```sh
npm install
```

```sh
npm start
```

2.  **Go to COntainer(host) application**

```sh
cd conatainer
```

```sh
npm install
```

```sh
npm start
```

3.  **Go to Marketing application**

```sh
cd marketing
```

```sh
npm install
```

```sh
npm start
```

2.  **Go to Dashboard application**

```sh
cd dashboard
```

```sh
npm install
```

```sh
npm start
```

| Microforntend | Ports                 | Feature                          |
| ------------- | --------------------- | -------------------------------- |
| Auth          | http://localhost:3002 | Handle authentication            |
| Container     | http://localhost:3000 | Integrate all microservices      |
| marketing     | http://localhost:3001 | Handle landing page and other .. |
| dashboard     | http://localhost:3003 | Handle dahboard after login      |

#### How to set up Auth0 in react js or Microfrontend

1. **Configuration Steps**
   **1.** Login into the Auth0 via Google, Github etc .,

   **2.** Create Application in Auth0 Dashboard
   -> Go to Auth0 Dashnoard
   -> Navigate to Applications > Application
   ![Step 1](./Readme/Image1.png)
   -> Click Create Application
   -> Name (ex., MicroApp)
   ->Choose Single Page Web Applications
   -> Click Create
   ![Step 2](./Readme/Image2.png)

2. **Configure Application Settings**
   -> Allowed Callback URLs:http://localhost:3002/callback , http://localhost:3000/callback(or wherever your Auth microfrontend runs)

   -> Allowed Logout URLs:http://localhost:3002 ,http://localhost:3000 ,http://localhost:3001

   -> Allowed Web Origins:http://localhost:3002 , http://localhost:3000 ,http://localhost:3001
   -> Save your Domain, Client ID

3. **Wrap Your App with Auth0Provider**
   -> Replace YOUR_DOMAIN and YOUR_CLIENTID with your values from the Auth0 dashboard.

```jsx
import { Auth0Provider } from "@auth0/auth0-react";
import React from "react";

export function AuthProvider({ children }) {
  const domain = "YOUR_DOMAIN";
  const clientId = "YOUR_CLIENTID";

  return (
    <Auth0Provider
      domain={domain}
      clientId={clientId}
      authorizationParams={{
        redirect_uri: `${window.location.origin}/callback`,
        audience: "https://localhost:6064",
        scope: "openid profile email read:basket",
      }}
    >
      {children}
    </Auth0Provider>
  );
}
```

4. **Use Auth0 in Application**
   **Login Section**

```jsx
import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Button } from "@mui/material";

const LoginButton = () => {
  const { loginWithRedirect } = useAuth0();

  const handleLogin = async () => {
    await loginWithRedirect();
  };

  return (
    <Button
      onClick={handleLogin}
      sx={{
        color: "white",
        backgroundColor: "transparent",
        border: "none",
        boxShadow: "none",
        "&:hover": {
          backgroundColor: "rgba(255, 255, 255, 0.1)",
        },
      }}
    >
      Log In
    </Button>
  );
};

export default LoginButton;
```

**Profile Section**

```jsx
import React, { useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import {
  Card,
  CardContent,
  Typography,
  Avatar,
  CircularProgress,
  Box,
} from "@mui/material";
import EmailIcon from "@mui/icons-material/Email";
import PersonIcon from "@mui/icons-material/Person";

const Profile = () => {
  const { user, isAuthenticated, isLoading, getAccessTokenSilently } =
    useAuth0();

  return (
    isAuthenticated && (
      <Box
        display="flex"
        justifyContent="center"
        alignItems="center"
        height="100vh"
        bgcolor="background.default"
        padding={2}
      >
        <Typography
          variant="body1"
          color="textSecondary"
          sx={{ display: "flex", alignItems: "center", marginTop: 1 }}
        >
          <EmailIcon sx={{ marginRight: 1 }} /> {user?.email}
        </Typography>
      </Box>
    )
  );
};

export default Profile;
```
