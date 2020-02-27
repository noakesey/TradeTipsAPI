# Introduction 
This is a work-in-progress web API developed using ASP.NET Core.  

It is based on the [ContosoUniversityCore](https://github.com/jbogard/ContosoUniversityCore) and [Realworld](https://github.com/gothinkster/aspnetcore-realworld-example-app) projects and uses CQRS with [MediatR](https://github.com/jbogard/MediatR) to continue thin controller / seperation of concern design patterns.

Authentication is implemented using JWT [ASP.NET Core JWT Bearer Authentication]

Some further refactoring is required with [AutoMapper](http://automapper.org) and LINQ to optimise the handlers.

# Getting started

Install the .NET Core SDK and lots of documentation: [https://www.microsoft.com/net/download/core](https://www.microsoft.com/net/download/core)

Documentation for ASP.NET Core: [https://docs.microsoft.com/en-us/aspnet/core/](https://docs.microsoft.com/en-us/aspnet/core/)

## Docker Build

There is a 'Makefile' for OS X and Linux:

- `make build` executes `docker-compose build`
- `make run` executes `docker-compose run`

The above might work for Docker on Windows

## Local building

- Install Cake as a global tool: `dotnet tool install -g Cake.Tool`
- Run Cake: `dotnet cake build.cake`
  - Note: this publishes as an OS X runtime by default.  Use the Cake argument `runtime` passing an RID explained here: https://docs.microsoft.com/en-us/dotnet/core/rid-catalog
  - The `Dockerfile` uses this to publish to Alpine

## Swagger URL
- `http://localhost:5000/swagger`
