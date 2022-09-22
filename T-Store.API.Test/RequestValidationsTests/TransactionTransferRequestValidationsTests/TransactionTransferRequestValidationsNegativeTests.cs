using FluentValidation.TestHelper;
using IncredibleBackendContracts.Requests;
using NUnit.Framework;
using T_Store.API.Test.RequestValidationsTests.ModelSource;
using T_Store.CustomValidations.FluentValidators;

namespace T_Store.API.Test.RequestValidationsTests.TransactionTransferRequestValidationsTests;

public class TransactionTransferRequestValidationsNegativeTests
{
    private TransactionTransferRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TransactionTransferRequestValidator();
    }

    [TestCaseSource(typeof(TransactionTransferRequestSource))]
    public void TransactionTransferRequest_SendingIncorrectData_GetErrorMessage(TransactionTransferRequest transfer, string errorMessage)
    {
        //given,when
        var isValid = _validator.TestValidate(transfer);

        //then
        Assert.False(isValid.IsValid);
        Assert.AreEqual(errorMessage, isValid.ToString());
    }
}
