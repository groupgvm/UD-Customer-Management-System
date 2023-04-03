using CM.DatabaseCreater;
using System.Configuration;
using CM.DatabaseCreater;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.



builder.Services.AddSingleton<CosmosService>();

var app = builder.Build();



app.MapGet("/", async 
    () => { return await new CosmosService(configuration).CreateContainers() });




app.Run();

