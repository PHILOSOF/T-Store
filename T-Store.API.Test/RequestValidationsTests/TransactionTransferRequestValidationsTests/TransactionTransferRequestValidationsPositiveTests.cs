using FluentValidation.TestHelper;
using IncredibleBackendContracts.Enums;
using IncredibleBackendContracts.Requests;
using NUnit.Framework;
using T_Store.CustomValidations.FluentValidators;

namespace T_Store.API.Test.RequestValidationsTests.TransactionTransferRequestValidationsTests;

public class TransactionTransferRequestValidationsPositiveTests
{
    private TransactionTransferRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TransactionTransferRequestValidator();
    }

    [Test]
    public void TransactionTransferRequest_SendingCorrectData_NoErrorReceived()
    {
        //given
        var client = new TransactionTransferRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 10,
            RecipientCurrency = Currency.JPY,
            RecipientAccountId = 3

        };

        //when
        var isValid = _validator.TestValidate(client);

        //then
        Assert.True(isValid.IsValid);
    }
}
