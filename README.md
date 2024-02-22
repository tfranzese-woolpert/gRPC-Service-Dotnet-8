# gRPC-Service-Dotnet-8

In this repo we build a gRPC service with 5 methods: Create, Read (single), List (multiple), Update and Delete. We then employ JSON transcoding (a new feature added in .NET 7) to allow our gRPC service to act as a REST based API. This allows web-based endpoints to consume our service, while at the same time continuing to allow native gRPC clients to consume as well. 

Reference Link: https://www.youtube.com/watch?v=Rqz9XiSqH3E
