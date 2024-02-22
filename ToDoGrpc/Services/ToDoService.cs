using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace ToDoGrpc;

public class ToDoService : ToDoIt.ToDoItBase
{
    private readonly AppDbContext _dbContext;

    public ToDoService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<CreateToDoResponse> CreateToDo(CreateToDoRequest request, ServerCallContext context)
    {
        if (request.Title == string.Empty || request.Description == string.Empty)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid object"));

        var toDoItem = new ToDoItem
        {
            Title = request.Title,
            Description = request.Description
        };

        await _dbContext.AddAsync(toDoItem);
        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new CreateToDoResponse
        {
            Id = toDoItem.Id
        });
    }

    public override async Task<ReadToDoResponse> ReadToDo(ReadToDoRequest request, ServerCallContext context)
    {
        if (request.Id <= 0)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Resource index must be greater than 0"));

        ToDoItem? toDoItem = await _dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (toDoItem != null)
        {
            return await Task.FromResult(new ReadToDoResponse
            {
                Id = toDoItem.Id,
                Title = toDoItem.Title,
                Description = toDoItem.Description,
                ToDoStatus = toDoItem.ToDoStatus
            });
        }

        throw new RpcException(new Status(StatusCode.NotFound, $"No Task with id {request.Id}"));
    }

    public override async Task<GetAllResponse> ListToDo(GetAllRequest request, ServerCallContext context)
    {
        GetAllResponse response = new();
        List<ToDoItem> toDoItems = await _dbContext.ToDoItems.ToListAsync();

        toDoItems.ForEach(x =>
        {
            response.ToDo.Add(new ReadToDoResponse()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ToDoStatus = x.ToDoStatus
            });
        });

        return await Task.FromResult(response);
    }

    public override async Task<UpdateToDoResponse> UpdateToDo(UpdateToDoRequest request, ServerCallContext context)
    {

        if (request.Id <= 0 || request.Title == string.Empty || request.Description == string.Empty)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid object"));


        ToDoItem? toDoItem = await _dbContext.ToDoItems.FirstOrDefaultAsync(x => x.Id == request.Id) 
            ?? throw new RpcException(new Status(StatusCode.NotFound, $"No Task with id {request.Id}"));
        
        toDoItem.Title = request.Title;
        toDoItem.Description = request.Description;
        toDoItem.ToDoStatus = request.ToDoStatus;

        await _dbContext.SaveChangesAsync();

        return await Task.FromResult(new UpdateToDoResponse
        {
            Id = toDoItem.Id
        });
    }
}