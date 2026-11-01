using Shopfront.Application;
using Shopfront.Infrastructure;
using Shopfront.Infrastructure.Persistence;
using Shopfront.WebApi.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.Services.InitializeDatabaseAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

public partial class Program
{
}
