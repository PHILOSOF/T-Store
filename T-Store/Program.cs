using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Data.SqlClient;
using T_Store.MapperConfig;
using T_Strore.Business.Services;
using T_Strore.Data.Repository;
using T_Store.Extensions;
using FluentValidation.AspNetCore;
using System.Reflection;
using T_Store.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

var conString = new ConnectionOption();
builder.Configuration.Bind(conString);

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(conString.TSRORE_DB_CONNECTION_STRING));

builder.Services.AddControllers()
    .AddFluentValidation(c => c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()))
    .AddNewtonsoftJson()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var result = new BadRequestObjectResult(context.ModelState);
            result.StatusCode = StatusCodes.Status422UnprocessableEntity;
            return result;
        };
        
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "T-Store", Version = "v1" 
    });         
});

builder.Services.AddScoped<ITransactionRepository, TransactionRepositories>();
builder.Services.AddScoped<ITransactionServices, TransactionServices>();
builder.Services.AddScoped<ICalculationServices, CalculationServices>();

builder.Services.AddAutoMapper(typeof(MapperConfigStorage));




var app = builder.Build();

app.UseCustomExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();