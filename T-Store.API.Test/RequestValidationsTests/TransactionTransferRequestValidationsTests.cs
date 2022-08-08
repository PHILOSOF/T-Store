using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using T_Store.API.Test.RequestValidationsTests.ModelSource;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store.API.Test.RequestValidationsTests;

public class TransactionTransferRequestValidationsTests
{
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

        var validationsResults = new List<ValidationResult>();

        //when
        var isValid = Validator.TryValidateObject(client, new ValidationContext(client), validationsResults, true);

        //then
        Assert.True(isValid);
    }

    [TestCaseSource(typeof(TransactionTransferRequestSource))]
    public async Task TransactionTransferRequest_SendingIncorrectData_GetErrorMessage(TransactionTransferRequest transfer, string errorMessage)
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
