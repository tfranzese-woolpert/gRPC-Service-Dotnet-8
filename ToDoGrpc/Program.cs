using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using ToDoGrpc;
using ToDoGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlite("DataSource=ToDoDatabase.db"));

// Add services to the container.
builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo { Title = "ToDo gRPC", Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "ToDoGrpc.xml");
    c.IncludeXmlComments(filePath);
    c.IncludeGrpcXmlComments(filePath, includeControllerXmlComments: true);
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    if (HttpMethods.IsPost(context.Request.Method) && context.Request.Path.StartsWithSegments(PathString.FromUriComponent("/v1/todo")))
    {
        var node = await JsonNode.ParseAsync(context.Request.Body);
        var obj = node?.AsObject();
        if (obj?.TryGetPropertyValue("travelMode", out JsonNode? propertyNode) is true && propertyNode?.GetValueKind() == JsonValueKind.String)
        {
            var value = propertyNode.GetValue<string>();
            var options = new JsonSerializerOptions
            {
                DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseUpper
            };
            var tempJson = JsonSerializer.Serialize(new Dictionary<string, string?> { { value, null } }, options);
            var convertedPropertyName = JsonSerializer.Deserialize<Dictionary<string, string?>>(tempJson)!.Keys.First()!;
            obj![propertyNode.GetPropertyName()] = convertedPropertyName;
        }

        var json = obj?.ToJsonString() ?? "{}";
        var buffer = Encoding.UTF8.GetBytes(obj?.ToJsonString() ?? "{}");
        context.Request.Body = new MemoryStream(buffer);
        context.Request.ContentLength = buffer.Length;
    }
    await next.Invoke();
});

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo gRPC V1");
});

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<ToDoService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
