using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using T_Store;
using T_Store.Middleware;
using T_Strore.Business.Services;
using T_Strore.Business.Services.Interfaces;
using T_Strore.Data;
using T_Strore.Data.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
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

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionServices, TransactionServices>();

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