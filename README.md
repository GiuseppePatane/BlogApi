# BlogPost API

This project contains the crud API for creating and retrieving a post of a hypothetical Blog.

### Database structure
+ **Author**: create the blog post. An author can create more than one blog post.
+ **Category**: the blog post category( programming, news etc.). A blog post can have only one category.
+ **Tag**:  key for group the blog posts. For example group all posts that talk about java. A blog post can have more than one tag and one tag can be in more than one blog post.
+ **Blogpost**: the main entity, handle the blog post title, content, image etc.

## Stack
+ Language: C#
+ Web Framework: [net core Web Api](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
+ ORM: [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
+ Database: PostgresSql


## Project structure


1)the *src* folder contains the source code of the application, all projects are grouped in the following project folders:
+ **Core**: it contains the libraries relating to the domain logic of the application. and do not contain references to external libraries
+ **Infrastructure**: it contains the libraries that interact with external services/libraries, such as the drivers of a specific database
+ **Api**: contains the web api project.


2) the test folder contains the libraries for testing the application code behaviour:
    + **BlogPost.UnitTest**:  project for testing the domain logic
    + **BlogPost.IntegrationTests**: project for testing the interaction between the domain logic and the database
    + **BlogPost.FunctionalTest** roject for testing the web API.  Starts a test web server and a test DB.


> N.B This project structure is visible only with one of the following IDE:
> * [Rider] (https://www.jetbrains.com/rider/)
> * [Visual studio] (https://visualstudio.microsoft.com/it/) windows and mac
> * [Visual studio code] (https://code.visualstudio.com/)  only with [this](https://marketplace.visualstudio.com/items?itemName=fernandoescolar.vscode-solution-explorer) plugin installed

## Run test with the DotNet CLI

1. Clone or download this repository to local machine.

   ` git clone https://github.com/GiuseppePatane/PokemonTranslator.git`

2. Install [.NET 6 SDK for your platform](https://www.microsoft.com/net/core#windowscmd) if didn't install yet.
3. Go to the project folder:` cd BlogApi`
4. Run `dotnet restore BlogApi.sln`
5. Start the postgres instance with `docker-compose -f docker-compose.yml up  postgres -d` 
6. Install the Report Generator as a global tool with `dotnet tool install --global dotnet-reportgenerator-globaltool`
7. Generate the xml coverage report with `dotnet test --collect:"XPlat Code Coverage" --results-directory:"./.coverage"`
8. Generate the html report  with `reportgenerator "-reports:.coverage/**/*.cobertura.xml" "-targetdir:.coverage-report/" "-reporttypes:HTML;"`
9. Open the file `.coverage-report/index.html` for see the full report.

> For zsh if reportgenerator comand not found, you'll need to modify your .zshrc manually to append ~/.dotnet/tools to PATH

## Run  docker container:

1. Clone or download this repository to a local machine.

   ` git clone https://github.com/GiuseppePatane/BlogApi.git`

2. Install [Docker for your platform](https://www.docker.com/get-started) if didn't install yet.

3. install [Docker-Compose](https://docs.docker.com/compose/install/)

4. ` cd BlogApi/`

5. run  `docker compose  -f docker-compose.yml up` for starting the application and:
    + an instance of PostgresSql
    + an instance of [seq](https://datalust.co/seq) to see the application log

6. for test the apis you may use  the file `testApi.http` with visual studio code  and the  [rest client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)
   > N.B the first run will create the database with some test data

7. to see the Swagger go to http://localhost:8090/index.html
8. to see the application logs go to seq at http://localhost:8888/#/events

## Code coverage
The code coverage is  93%

<img width="579" alt="image" src="https://user-images.githubusercontent.com/13527363/153725081-a826c7c7-b9ac-4726-aeed-c07535eb7dc0.png">
