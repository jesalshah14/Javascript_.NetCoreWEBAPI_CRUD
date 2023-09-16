using NotesAPI.Data;
using Microsoft.EntityFrameworkCore;
using NotesAPI;
using NotesAPI.Middileware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<NotesDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("NotesDbConnectionString"))); //we have inject db context inside services of di container of .net core app

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlerMW>(); // Add this global error handler

app.UseHttpsRedirection();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

//app.UseMiddleware<NotFoundMiddleware>(); // Add this line

app.MapControllers();

app.Run();
