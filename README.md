[![Build Status](https://travis-ci.org/unrealnerd/restfulprofiles.svg?branch=master)](https://travis-ci.org/unrealnerd/restfulprofiles)

# Usage
Clone the source code from [Github](https://github.com/unrealnerd/restfulprofiles)

`git clone https://github.com/unrealnerd/restfulprofiles.git`

Copy the zipped data folder to `<clonedpath>\restfulprofiles\data`

Start the mongo shell using the below command

`c:\mongodb\mongod.exe --dbapth "<clonedpath>\restfulprofiles\data"`

Now open the `restfulprofiles\` directory with vscode

run `dotnet restore`

press `f5` and run the project

If Using [VSCode Rest Client](https://marketplace.visualstudio.com/itemdetails?itemName=humao.rest-client), just run the requests in order.

If Using postman collection from tools folder get the JWT token and set the environment variable `APIKey` with the reponse token. Or chnage the Authorization header in each request.

# Using container
After cloning and copying the data to data folder

Open powershell in run `docker-compose up --build`

Now use the tools/*.http file to create an apikey and then use the Profiles/Api


# NOTES

# Things Done
* basic CRUD apis
* configure mongodb
* mongodb integration with the API
* inject configrations as options
* upload to git
* Move all creation of instances to DI
* create postman collection
* Use Repository Pattern
* Use generics so that Irepository can be re-used 
* Resolve mongodb complex object(Address in Profile) serialization and de-serialization
* make all async
* unit test on controller 
* Create middleware for global exception handling
* Add security for API- try using JWT instead of cookie
for generating token use "api/Login" and generate a Token. Then use this token in the request header
* case insensitive search using Query
* rest client tool created
* search action implementation
* containerize using docker

# Things to be Done

* model validation for querybuilder
* checkout text-indexes in mongodb 
* seperate API proj and lib proj
* unit test- Use Autofixture
* Integrate swagger 
* unit test for repository
* Integrate Logging service
* deploy on heroku or azure
* host sample mongodb in free host
* seperate data and api docker containers
* perforamnce factors- use caching
* search/ query using simpler query input as string or regular expression
* add patch endpoint
* use LoggerMessage for performant loggiing instead of logger extension
* create set up scripts
* use .netcore secrets to store apikeys
* set username and passwod for mongodb
* swarm of containers and handling db in that scenario

### Assumptions
* made use of new id(string- objectID) instead of simple integer column userId with  for mongodb integration
* only supporting english Language
