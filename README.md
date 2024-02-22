# gRPC-Service-Dotnet-8

In this repo we build a gRPC service with 5 methods: Create, Read (single), List (multiple), Update and Delete. We then employ JSON transcoding (a new feature added in .NET 7) to allow our gRPC service to act as a REST based API. This allows web-based endpoints to consume our service, while at the same time continuing to allow native gRPC clients to consume as well. 

Reference Link: https://www.youtube.com/watch?v=Rqz9XiSqH3E

# Create project

dotnet new grpc -o ToDoGrpc

# Install package dependencies

dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Grpc.Tools
dotnet add package Microsoft.AspNetCore.Grpc.JsonTranscoding

# Install Cross Platform Database Tool

https://dbeaver.io/

# gRPC JSON transcoding in ASP.NET Core

https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding?view=aspnetcore-7.0#http-protocol&WT.mc_id=DX-MVP-5004571