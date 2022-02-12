# BlogPost API

This project contains the crud api for create and retrieve a post of a hypothetical Blog.

### Database structure
+ **Author**: create the blog post. An author can create more that one  blog posts.
+ **Category**: The blog post category( programming, news ecc.). A Blog post can have only one category.
+ **Tag**: a Key for group the blog posts. For example group all posts that talk about java.A Blog post can have more that one tag that and one tag  can be in more that one blog post.
+ **Blogpost**: The main entity, handle the blog post title, content, image ecc

## Stack 
+ Language: C#
+ Web Framework: [net core Web Api](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
+ ORM: [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
+ Database: Postgres


## Project structure


1) *src* folder contains the source code of the application, all project are grouped  in the following  project folders:
    + **Core** : it contains the libraries relating to the domain logic of the application. and do not contain references to external libraries
    + **Infrastructure**  => it contains the libraries that interact with external services/libraries, such as the drivers of a specific database
    + **Api** => contains the web api project.


2) test folder contains the libraries for testing the application code behavior 
   + **BlogPost.UnitTest**:  project for testing the domain logic 
   + **BlogPost.IntegrationTests**: project for testing the interaction between the domain logic and the database
   + **BlogPost.FunctionalTest** project for testing the web api. It's start a test web server and a test db.

          
> N.B this  project structure in visible only with one of the following idee: 
> * [Rider] (https://www.jetbrains.com/rider/) 
> * [Visual studio] (https://visualstudio.microsoft.com/it/) windows and mac  
> * [Visual studio code] (https://code.visualstudio.com/)  only with [this](https://marketplace.visualstudio.com/items?itemName=fernandoescolar.vscode-solution-explorer) plugin installed


## Run  docker container:

1. Clone or download this repository to local machine.

   ` git clone https://github.com/GiuseppePatane/BlogApi.git`

2. Install [Docker for your platform](https://www.docker.com/get-started) if didn't install yet.

3. install [Docker-Compose](https://docs.docker.com/compose/install/)

4. ` cd BlogApi/`

5. run  `docker compose  -f docker-compose.yml up`

6. for test the apis  you may use  the file `testApi.http` with visual studio code  and the  [rest client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client)

7. For see the Swagger go to http://localhost:8090/swagger/index.html
8. For see the application logs go to [seq](https://datalust.co/seq) http://localhost:8888/#/events

## Code coverage 
The code coverage is  93% 

<img width="579" alt="image" src="https://user-images.githubusercontent.com/13527363/153725081-a826c7c7-b9ac-4726-aeed-c07535eb7dc0.png">
