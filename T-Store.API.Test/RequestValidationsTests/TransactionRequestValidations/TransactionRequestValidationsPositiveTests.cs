using FluentValidation.TestHelper;
using IncredibleBackendContracts.Enums;
using IncredibleBackendContracts.Requests;
using NUnit.Framework;
using T_Store.CustomValidations.FluentValidators;

namespace T_Store.API.Test.RequestValidationsTests.TransactionRequestValidations;

public class TransactionRequestValidationsTests
{
    private TransactionRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TransactionRequestValidator();
    }

    [Test]
    public void TransactionRequest_SendingCorrectData_NoErrorReceived()
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
}
