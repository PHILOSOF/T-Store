using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;
using System.Data;
using System.Data.SqlClient;
using T_Store.Extensions;
using T_Store.Infrastructure;
using T_Store.MapperConfiguration;
using T_Strore.Business.MapperConfiguration;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

var dbConfig = new DbConfig();
builder.Configuration.Bind(dbConfig);

LogManager.Configuration.Variables[$"{ environment: LOG_DIRECTORY}"] = "Logs";

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(dbConfig.TSTORE_DB_CONNECTION_STRING));

builder.Services.AddControllers()
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

builder.Services.AddMassTransit();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidation();
builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddProducers();
builder.Services.AddAutoMapper(typeof(MapperConfigBusiness), typeof(MapperConfigAPI));
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