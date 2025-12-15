using Evently.Api;
using Evently.Api.Extensions;
using Evently.Modules.Events.Infrastructure;
using Evently.Shared.Application;
using Evently.Shared.Infrastructure;
using Evently.Shared.Presentation;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, logger) => logger.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger => 
    swagger.CustomSchemaIds(type => type.FullName!.Replace('+', '.')));


builder.Services.AddEventlyApplication([Evently.Modules.Events.Application.AssemblyReference.Assembly]);

string dbConnectionString = builder.Configuration.GetConnectionString("EventsDatabase")!;
string redisConnectionString = builder.Configuration.GetConnectionString("Redis");
builder.Services.AddEventlyInfrastructure(dbConnectionString, redisConnectionString);

builder.Services.AddEventsModule(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Database Migrations
    app.ApplyMigrations();
}

app.UseSerilogRequestLogging();
app.MapEndPoints();
app.UseExceptionHandler();

await app.RunAsync();
