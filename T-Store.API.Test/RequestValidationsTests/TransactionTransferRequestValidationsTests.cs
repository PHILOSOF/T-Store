using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using T_Store.API.Test.RequestValidationsTests.ModelSource;
using T_Store.Models;
using T_Strore.Data;
using FluentValidation.TestHelper;
using T_Store.CustomValidations.FluentValidators;

namespace T_Store.API.Test.RequestValidationsTests;

public class TransactionTransferRequestValidationsTests
{
    private TransactionTransferRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TransactionTransferRequestValidator();

    }

    [Test]
    public async Task TransactionTransferRequest_SendingCorrectData_GetAnEmptyStringError()
    {
        //given
        var client = new TransactionTransferRequest()
        {
            AccountId =1,
            Currency= Currency.USD,
            Amount=10,
            RecipientCurrency= Currency.JPY,
            RecipientAccountId=3
            
        };

        //when
        var isValid = _validator.TestValidate(client);

        //then
        Assert.True(isValid.IsValid);
    }

    [TestCaseSource(typeof(TransactionTransferRequestSource))]
    public async Task TransactionTransferRequest_SendingIncorrectData_GetErrorMessage(TransactionTransferRequest transfer, string errorMessage)
    {
        //given,when
        var isValid = _validator.TestValidate(transfer);

        //then
        Assert.False(isValid.IsValid);
        Assert.AreEqual(errorMessage, isValid.ToString());
    }
}
