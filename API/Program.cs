using Infrastructure.Data;
using Infrastructure.ExtensionServices;
using Microsoft.EntityFrameworkCore;
using ShopeStore.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddCustomServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseStatusCodePagesWithReExecute("/NotFound/{0}"); //{0} is placeholder and errors is the name of the controller /ApiError/404
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

/*create the Migration if there is no Migration*/
using var scope = app.Services.CreateScope();
var servics = scope.ServiceProvider;
var context = servics.GetRequiredService<StoreContext>();
var logger = servics.GetRequiredService<ILogger<Program>>();
try

{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

app.Run();

