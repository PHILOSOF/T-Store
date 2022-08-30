﻿using FluentValidation.TestHelper;
using NUnit.Framework;
using T_Store.API.Test.RequestValidationsTests.ModelSource;
using T_Store.CustomValidations.FluentValidators;
using T_Store.Models;

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
    public async Task TransactionTransferRequest_SendingIncorrectData_GetErrorMessage(TransactionTransferRequest transfer, string errorMessage)
    {
        //given,when
        var isValid = _validator.TestValidate(transfer);

        //then
        Assert.False(isValid.IsValid);
        Assert.AreEqual(errorMessage, isValid.ToString());
    }
}
