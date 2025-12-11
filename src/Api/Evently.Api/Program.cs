using Evently.Api.Extensions;
using Evently.Modules.Events.Infrastructure;
using Evently.Shared.Application;
using Evently.Shared.Infrastructure;
using Evently.Shared.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger => 
    swagger.CustomSchemaIds(type => type.FullName!.Replace('+', '.')));


builder.Services.AddEventlyApplication([Evently.Modules.Events.Application.AssemblyReference.Assembly]);

string dbConnectionString = builder.Configuration.GetConnectionString("EventsDatabase")!;

builder.Services.AddEventlyInfrastructure(dbConnectionString);

builder.Services.AddEventsModule(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Database Migrations
    app.ApplyMigrations();
}

app.MapEndPoints();

await app.RunAsync();
