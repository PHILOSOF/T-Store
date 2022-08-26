using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using T_Store.Extensions;
using T_Store.Infrastructure;
using T_Store.MapperConfig;
using T_Strore.Business.Services;
using T_Strore.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

var conOptions = new ConnectionOption();

builder.Configuration.Bind(conOptions);
LogManager.Configuration.Variables[$"{ environment: LOG_DIRECTORY}"] = "Logs";

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(conOptions.TSRORE_DB_CONNECTION_STRING));

// to fix
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

// move code into extension methods
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
builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Logging.ClearProviders();
builder.Host.UseNLog();
      
var app = builder.Build();
app.UseCustomExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseAdminSafeList();

app.Run();