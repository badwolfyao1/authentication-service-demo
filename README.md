# Authentication Service Demo (with JWT)
An Authentication Service Demo in .NET Core. 
## Objective
This demo shows how to build an authentication service in dotnet core that use JWT token. In this demo, we use basic auth to authenticate the user and generate a JWT token for such user, and then use Bearer token to check whether the token is valid or invalid. 

## Repository

This repository has following parts,
+ **Controllers**
  * **AuthController** This controller has two endpoints,
    - **POST** - POST method is used to perform login action, pass in username and password to get an JWT access token
    - **GET** - GET method is used to verify the JWT token, return 200OK to indicate the request is authenticated
+ **Services**
  * **JWTService** has two methods, 
    - **CreateToken** will accept a username and create a JWT token and return it, the secret is hard coded in JWTServce 
    - **VerifyToken** will accept a JWT token and a username, then it will validate the JWT token based on the username, expiration time, secret mentioned above
  * **UserService** Currently Empty, but if you want to expend the functionality of this service and let it able to signup for users, that's a way to go
+ **Docker**
  * **Dockerfile** This is the file docker use to build the image
  * **docker-compose.yml** This is the compose file

  
