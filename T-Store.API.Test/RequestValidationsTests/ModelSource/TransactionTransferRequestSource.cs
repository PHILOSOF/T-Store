using System.Collections;
using T_Store.Infrastructure;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store.API.Test.RequestValidationsTests.ModelSource;

public class TransactionTransferRequestSource : IEnumerable
{
    public TransactionTransferRequest GetTransactionTransferRequestModelForTests()
    {
        return new TransactionTransferRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 10,
            RecipientCurrency = Currency.JPY,
            RecipientAccountId = 3
        };
    }

    public IEnumerator GetEnumerator()
    {
        var model = GetTransactionTransferRequestModelForTests();
        model.AccountId = 0;
        yield return new object[]
        {
            model,
            ApiErrorMessage.NumberLessOrEqualZero
        };

        model = GetTransactionTransferRequestModelForTests();
        model.Amount = 0;
        yield return new object[]
        {
            model,
            ApiErrorMessage.NumberLessOrEqualZero
        };

        model = GetTransactionTransferRequestModelForTests();
        model.RecipientAccountId = 0;
        yield return new object[]
        {
            model,
            ApiErrorMessage.NumberLessOrEqualZero
        };

        model = GetTransactionTransferRequestModelForTests();
        model.AccountId = -1;
        yield return new object[]
        {
            model,
            ApiErrorMessage.NumberLessOrEqualZero
        };

        model = GetTransactionTransferRequestModelForTests();
        model.Amount = -1;
        yield return new object[]
        {
            model,
            ApiErrorMessage.NumberLessOrEqualZero
        };

        model = GetTransactionTransferRequestModelForTests();
        model.RecipientAccountId = -1;
        yield return new object[]
        {
            model,
            ApiErrorMessage.NumberLessOrEqualZero
        };

        model = GetTransactionTransferRequestModelForTests();
        model.Currency = 0;
        yield return new object[]
        {
            model,
            ApiErrorMessage.CurrencyRangeError
        };

        model = GetTransactionTransferRequestModelForTests();
        model.RecipientCurrency = 0;
        yield return new object[]
        {
            model,
            ApiErrorMessage.CurrencyRangeError
        };

        model = GetTransactionTransferRequestModelForTests();
        model.RecipientCurrency = Currency.USD;
        model.Currency = Currency.USD;
        yield return new object[]
        {
            model,
            ApiErrorMessage.SameCurrency
        };
    }
}
