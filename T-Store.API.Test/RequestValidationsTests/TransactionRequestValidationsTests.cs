

using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using T_Store.API.Test.RequestValidationsTests.ModelSource;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store.API.Test.RequestValidationsTests;

public class TransactionRequestValidationsTests 
{
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

        var validationsResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationsResults, true);

        //then
        Assert.True(isValid);
    }

    [TestCaseSource(typeof(TransactionRequestSource))]
    public async Task TransactionRequest_SendingIncorrectData_GetErrorMessage(TransactionRequest transfer, string errorMessage)
    {
        //given
        var validationsResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(transfer, new ValidationContext(transfer), validationsResults, true);

        //then
        var actualMessage = validationsResults[0].ErrorMessage;
        Assert.AreEqual(errorMessage, actualMessage);
    }

}
