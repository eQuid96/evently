using Evently.Api.Extensions;
using Evently.Modules.Events.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger => 
    swagger.CustomSchemaIds(type => type.FullName!.Replace('+', '.')));

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

EventsModule.MapEndPoints(app);

await app.RunAsync();
