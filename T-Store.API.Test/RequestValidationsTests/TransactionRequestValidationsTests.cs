using NUnit.Framework;
using T_Store.API.Test.RequestValidationsTests.ModelSource;
using T_Store.CustomValidations.FluentValidators;
using T_Store.Models;
using T_Strore.Data;
using FluentValidation.TestHelper;


namespace T_Store.API.Test.RequestValidationsTests;

public class TransactionRequestValidationsTests 
{
    private TransactionRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TransactionRequestValidator();
        
    }

    [Test]
    public async Task TransactionRequest_SendingCorrectData_GetAnEmptyStringError()
    {
        //given
        var client = new TransactionRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 10,
        };

        //when
        var isValid = _validator.TestValidate(client);

        //then
        Assert.True(isValid.IsValid);
    }

    [TestCaseSource(typeof(TransactionRequestSource))]
    public async Task TransactionRequest_SendingIncorrectData_GetErrorMessage(TransactionRequest transfer, string errorMessage)
    {

        //given,when
        var isValid = _validator.TestValidate(transfer);

        //then
        Assert.False(isValid.IsValid);
        Assert.AreEqual(errorMessage, isValid.ToString());
    }
}
