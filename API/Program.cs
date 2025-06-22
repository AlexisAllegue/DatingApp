using API.Interfaces;
using API.Services;
using API.Extensions;
using API.Middleware;
using API.Errors;
var builder = WebApplication.CreateBuilder(args);

// Middleware
//Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

//add Authentication
builder.Services.AddApplicationAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
