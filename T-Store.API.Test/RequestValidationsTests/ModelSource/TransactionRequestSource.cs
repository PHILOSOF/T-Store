using IncredibleBackendContracts.Enums;
using System.Collections;
using T_Store.Infrastructure;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store.API.Test.RequestValidationsTests.ModelSource;

public class TransactionRequestSource : IEnumerable
{
    public TransactionRequest GetTransactionTransferRequestModelForTests()
    {
        return new TransactionRequest()
        {
            AccountId = 1,
            Currency = Currency.USD,
            Amount = 10,

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
        model.Currency = 0;
        yield return new object[]
        {
            model,
            ApiErrorMessage.CurrencyRangeError
        };
    }
}
