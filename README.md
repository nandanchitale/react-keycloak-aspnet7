# Overview:
This repository contains the source code for a full-stack web application built using React for the frontend, Keycloak as the authentication middleware, and ASP.NET 7 for the backend. The project aims to provide a robust and secure solution for developing modern web applications with seamless user authentication and authorization capabilities.

# Features:
- React Frontend: Utilizes React, a popular JavaScript library for building user interfaces, to create a dynamic and responsive frontend user experience.
- Keycloak Middleware: Integrates Keycloak, an open-source identity and access management solution, to manage user authentication, authorization, and single sign-on capabilities securely.
- ASP.NET 7 Backend: Employs ASP.NET 7, a powerful web framework for building scalable and high-performance backend services, to handle business logic, data processing, and API endpoints.

# Key Components:
- Frontend: The frontend directory contains the React application source code, including components, views, styles, and routing configurations.
- Middleware: The middleware directory houses the Keycloak configuration files, client settings, and authentication middleware for securing backend APIs and routes.
- Backend: The backend directory hosts the ASP.NET 7 backend application, including controllers, services, models, database interactions, and authentication endpoints.
- Dockerfiles and Docker Compose: The repository includes Dockerfiles and a Docker Compose file for containerizing the application components, enabling easy deployment and scalability.

# Keycloak Installation (Docker):
Ensure you have Docker installed on your machine. If not, download and install Docker Desktop from the official website.
Navigate to the middleware directory in the repository.
Open a terminal window and run the following command to start Keycloak using Docker Compose:
``` docker-compose up --build -d ```
Keycloak will start running in a Docker container. Access the Keycloak Admin Console by navigating to http://localhost:8181/ in your web browser.
Log in to the Admin Console using the default credentials (username: admin, password: admin). You can customize the admin credentials and other settings in the docker-compose.yml file.

# Getting Started:
Frontend : 
- Clone the repository to your local machine.
- Navigate to the frontend directory and install dependencies using npm install / yarn install.
- Start the React development server using npm start / yarn start.
Backend : 
- Configure and run the ASP.NET 7 backend application.
- Configure Keycloak settings and integrate authentication middleware with the backend APIs.
- Customize and extend the application based on your requirements.

