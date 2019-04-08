# Usage


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

# Things to be Done

* model validation for querybuilder
* checkout text-indexes in mongodb 
* seperate API proj and lib proj
* unit test- Use Autofixture
* Integrate swagger 
* unit test for repository
* Integrate Logging service
* containerize using docker
* deploy on heroku or azure
* host sample mongodb in free host
* seperate data and api docker containers
* perforamnce factors- use caching
* search/ query using simpler query input as string or regular expression
* add patch endpoint
* use LoggerMessage for performant loggiing instead of logger extension

### Assumptions
* made use of new id(string- objectID) instead of simple integer column userId with  for mongodb integration
* only supporting english Language
