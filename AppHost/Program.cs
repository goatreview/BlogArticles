using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<WebAPI>("WebAPI");

builder.Build().Run();
