using T_Strore.Data;

var testRep = new TransactionRepository();






var testDto = new TransactionDTO()
{

    AccountId = 3,

    TransactionType = TransactionType.Transfer,
    Amount = 100,
    Currency = Currency.USD,
};

var testMethod = testRep.AddTransaction(testDto);

Console.WriteLine();