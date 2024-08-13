using Microsoft.EntityFrameworkCore;
using testAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<taskDb>(opt => opt.UseInMemoryDatabase("taskList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// http endpoints
//get all tasks
app.MapGet("/tasks", async (taskDb db) => await db.Tasks.ToListAsync());

//get tasks by status
app.MapGet("/tasks/due", async (taskDb db) => await db.Tasks.Where(t => (t.Status == Stat.due)).ToListAsync());

app.MapGet("/tasks/onprogress", async (taskDb db) => await db.Tasks.Where(t => (t.Status == Stat.onProgress)).ToListAsync());

app.MapGet("/tasks/complete", async (taskDb db) => await db.Tasks.Where(t => (t.Status == Stat.done)).ToListAsync());

//get task by id
app.MapGet("/tasks/{id}", async (int id, taskDb db) =>
    await db.Tasks.FindAsync(id) is task task ? Results.Ok(task) : Results.NotFound());

//add task
app.MapPost("/tasks", async (task task, taskDb db) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();

    return Results.Created($"/taskitems/{task.Id}", task);
});

//update task
app.MapPut("/tasks/{id}", async (int id, task inputtask, taskDb db) =>
{
    var task = await db.Tasks.FindAsync(id);

    if (task is null) return Results.NotFound();

    task.Name = inputtask.Name;
    task.Status = inputtask.Status;

    await db.SaveChangesAsync();

    return Results.NoContent();
});


//delete task
app.MapDelete("/taskitems/{id}", async (int id, taskDb db) =>
{
    if (await db.Tasks.FindAsync(id) is task task)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();