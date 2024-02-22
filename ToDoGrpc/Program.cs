using Microsoft.EntityFrameworkCore;
using ToDoGrpc;
using ToDoGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlite("DataSource=ToDoDatabase.db"));

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ToDoService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
