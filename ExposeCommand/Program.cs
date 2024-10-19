using ExposeCommand;
using ExposeCommand.AddGoat;
using ExposeCommand.GetGoat;
using MediatR;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new
                {
                    Field = e.Key,
                    Errors = e.Value.Errors.Select(err =>
                    {
                        if (err.ErrorMessage.Contains("BirthDate"))
                            return "Birth date is invalid.";
                        return err.ErrorMessage;
                    })
                }).ToArray();

            return new BadRequestObjectResult(new
            {
                Message = "Validation failed",
                Errors = errors
            });
        };
    });



var app = builder.Build();

app.MapGet("/goats/{id}", async ([AsParameters] GetGoatQuery query, IMediator mediator) =>
{
    var result = await mediator.Send(query);
    return Results.Ok(result);
});

app.MapPost("/goats", async ([FromBody] AddGoatCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Created(result.Id.ToString(), result);
});

app.MapPost("/goatswithinput", async ([FromBody] AddGoatInput input, IMediator mediator) =>
{
    try
    {
        var command = AddGoatCommand.From(input);
        var result = await mediator.Send(command);
        return Results.Created(result.Id.ToString(), result);
    }
    catch (InternalValidationException e)
    {
        var error = new
        {
            e.Field,
            Error = e.Message
        };
        return Results.BadRequest(new
        {
            Message = "Validation failed",
            Errors = error
        });
    }
});

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();

// Needed for WebApplicationFactory
public partial class Program;