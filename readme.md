# EverySearch
It's asp net core based web app for web searching.
It uses 3 search engines in the same time through their API. API docs:
  * [Google Custom Search](https://developers.google.com/custom-search)
  * [Bing Search](https://docs.microsoft.com/en-us/azure/cognitive-services/bing-web-search/)
  * [Yandex.XML](https://xml.yandex.ru/)
## Project structure
Project has standard MVC structure, i.e. _Models_, _Views_ and _Controllers_ folders. It has also _Lib_ folder, that contains additional logic.
### Models
* Search

Search class contains search query, time it was sent and list of results (_SearchResult_).

* SearchResult

It contains search result url, title, piece of site content and link to _Search_ object.

### Views

* New

Form for input a search query.
* Show

It displays search results and provides input of new query.

* Index

It shows all search results from database. Results can be filtered by result title and search query.

* Error

It is shown when any error occurs.

### Controllers
* SearchesController

  Actions:
  - New
  
  It just renders corresponding view.

  - CreateAsync

  This action gets query string from _New_ view, performs request, saves search results to database and displays search results by redirect to _Show_ view.

   - ShowAsync

  _ShowAsync_ displays results of concrete search by it's Id unles it is empty. In this case it renders _Error_ view.


### Lib
* SearchProvider

  This class defines API provider interface. It has 3 abstract methods: 
  * _InitializeCredentials_ is called once. It allows derived class get API keys or any other data from configuration. 
  * _MakeRequest_ method should produce _HttpWebRequest_ object that will be used for API call. Result of this API call then will be passed to _ParseResponse_. 
  * _ParseResponse_ method is responsible for converting request result from json/xml form into collection of _SearchResult_ object.
* BingProvider, GoogleProvider and YandexProvider

_SearchProvider_ implementations for different APIs.

* SearchManager

This class is dependency of _SearchesController_. It runs request execution on API providers and returns result of first finished one.

* Utils

Contains some utilities.

## Adding new API provider

1. Create class that derives from _SearchProvider_
2. Add abstract methods implementations.
3. In constructor of _SearchManager_ add new provider instance to _searchProviders_ field.
4. Add API key to configuration file.

## Deployment
### Create docker container with SQL Server.
1. Pull the SQL Server 2019 Linux container image from Docker Hub.
```bash
docker pull mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
```
2. Run container from Docker image.
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=@Password123" -p 1433:1433 --name sql -d mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
```
3. Acess sql command line.
```bash
docker exec -it sql "bash"
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "@Password123"
```
4. Create database. 
```sql
1> CREATE DATABASE EverySearchDb
2> GO
1> exit
```
Then exit from bash by Ctrl+Z

### Change connection string
1. Get SQL Server IP
```bash
docker network inspect bridge
```
Output will contain this. There may be more than one containers. So look at "Name" property.
```json
...
        "Containers": {
            "afb766db2a8d6ffc29196ac52f7be81e60db7caad17582d44838732d1a262c9b": {
                "Name": "sql",
                "EndpointID": "76853b79cf2667c82d8b1a0be616d6b97018464cb931044c07d7637e258ff385",
                "MacAddress": "02:42:ac:11:00:02",
                "IPv4Address": "172.17.0.2/16",
                "IPv6Address": ""
            }
        },
...
```
Copy value of "IPv4Address" without "/16". It will refered in the next step as <sql_ip>.

2. Set connection string

Open EverySearch/appsettings.Development.json
```json
  "ConnectionStrings": {
    
  }
```
Add item "docker" to _ConnectionStrings_. It will look like this. 
```json
"ConnectionStrings": {
    "docker": "Server=172.17.0.2;Database=EverySearchDb;User ID=sa;Password=@Password123;"
}
```
3. Add API keys and other API configuration data to appsettings.Development.json:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "docker": "Server=172.17.0.2;Database=EverySearchDb;User ID=sa;Password=@Password123;"
  },
  "Bing": {
    "key": "<api_key>"
  },
  "Google": {
      "key": "<api_key>",
      "cx": "<cx>"
  },
  "Yandex":{
      "key": "<api_key>",
      "user": "<user>"
  }
}
```
4. Run app from Visual Studio

Change launch plarform on standard toolbar to "Docker" and press "Run"


