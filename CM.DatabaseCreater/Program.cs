using CM.DatabaseCreater;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.



builder.Services.AddSingleton<CosmosService>();

var app = builder.Build();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

