using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using T_Store.Middleware;
using T_Strore.Business.Exceptions;

namespace T_Store.API.Test.MiddlewareTests;

public class CustomExceptionHandlerMiddlewareTests
{
    private DefaultHttpContext _context;
    private Mock<ILogger<CustomExceptionHandlerMiddleware>> _logger;


    private CustomExceptionHandlerMiddleware _sut;
    [SetUp]
    public void Setup()
    {
        _context = new DefaultHttpContext();
        _logger = new Mock<ILogger<CustomExceptionHandlerMiddleware>>();
        
    }

    [Test]
    public async Task Invoke_ValidRequestPassed_()
    {
        //given
        RequestDelegate mockNextMiddleware = (HttpContext) =>
        {
            return Task.CompletedTask;
        };
        var exceptionHandlingMiddleware = new CustomExceptionHandlerMiddleware(mockNextMiddleware, _logger.Object);

        //when
        await exceptionHandlingMiddleware.Invoke(_context);

        //then
        Assert.AreEqual(200, _context.Response.StatusCode);
    }

    [Test]
    public async Task Invoke_InvalidPassed_ThrowBalanceExceedException()
    {
        //given
        var expectedException = new BalanceExceedException("Balance exceed exception");
        RequestDelegate mockNextMiddleware = (HttpContext) =>
        {
            return Task.FromException(expectedException);
        };

        var exceptionHandlingMiddleware = new CustomExceptionHandlerMiddleware(mockNextMiddleware, _logger.Object);

        //when
        await  exceptionHandlingMiddleware.Invoke(_context);

        //then
         Assert.AreEqual(400, _context.Response.StatusCode);
    }

    [Test]
    public async Task Invoke_InvalidPassed_ThrowEntityNotFoundException()
    {
        //given
        var expectedException = new EntityNotFoundException("Entity not found exception");
        RequestDelegate mockNextMiddleware = (HttpContext) =>
        {
            return Task.FromException(expectedException);
        };

        var exceptionHandlingMiddleware = new CustomExceptionHandlerMiddleware(mockNextMiddleware, _logger.Object);

        //when
        await exceptionHandlingMiddleware.Invoke(_context);

        //then
        Assert.AreEqual(404, _context.Response.StatusCode);
    }

    [Test]
    public async Task Invoke_InvalidPassed_ThrowServiceUnavailableExceptionn()
    {
        //given
        var expectedException = new ServiceUnavailableException("Service unavailable exception");
        RequestDelegate mockNextMiddleware = (HttpContext) =>
        {
            return Task.FromException(expectedException);
        };

        var exceptionHandlingMiddleware = new CustomExceptionHandlerMiddleware(mockNextMiddleware, _logger.Object);

        //when
        await exceptionHandlingMiddleware.Invoke(_context);

        //then
        Assert.AreEqual(503, _context.Response.StatusCode);
    }
}

