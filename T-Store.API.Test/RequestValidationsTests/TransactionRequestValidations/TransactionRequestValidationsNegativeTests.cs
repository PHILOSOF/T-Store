using FluentValidation.TestHelper;
using IncredibleBackendContracts.Requests;
using NUnit.Framework;
using T_Store.API.Test.RequestValidationsTests.ModelSource;
using T_Store.CustomValidations.FluentValidators;

namespace T_Store.API.Test.RequestValidationsTests.TransactionRequestValidations;

public class TransactionRequestValidationsNegativeTests
{
    private TransactionRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new TransactionRequestValidator();
    }

    [TestCaseSource(typeof(TransactionRequestSource))]
    public void TransactionRequest_SendingIncorrectData_GetErrorMessage(TransactionRequest transfer, string errorMessage)
    {
        //given,when
        var isValid = _validator.TestValidate(transfer);

        //then
        Assert.False(isValid.IsValid);
        Assert.AreEqual(errorMessage, isValid.ToString());
    }
}
