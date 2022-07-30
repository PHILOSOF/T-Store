using Dapper;


namespace T_Strore.Data
{
    public class TransactionRepository : BaseRepository
    {

        public int AddTransaction(TransactionDTO transaction)
        {
            var id = ConString.QueryFirstOrDefault<int>(
                      TransactionStoredProcedure.Transaction_Insert,
                      param: new
                      {
                          transaction.AccountId,
                          transaction.TransactionType,
                          transaction.Amount,
                          transaction.Currency
                      },
                      commandType: System.Data.CommandType.StoredProcedure);

            return id;
        }


        public decimal GetBalanceByAccountId(int accountId)
        {
            var balance = ConString.QueryFirstOrDefault<decimal>(
                     TransactionStoredProcedure.Transaction_SelectBalanceByAccountId,
                     param: new { accountId },
                     commandType: System.Data.CommandType.StoredProcedure);

            return balance;
        }


        public List<TransactionDTO> GetTransactionsByAccountId(int accountId)
        {
            var transaction = ConString.Query<TransactionDTO>(
                      TransactionStoredProcedure.Transaction_SelectByAccountId,
                      param: new { accountId },
                      commandType: System.Data.CommandType.StoredProcedure).ToList();

            return transaction;
        }


        public TransactionDTO GetTransactionById(int id)
        {
            var transaction = ConString.QueryFirstOrDefault<TransactionDTO>(
                     TransactionStoredProcedure.Transaction_SelectById,
                     param: new { id },
                     commandType: System.Data.CommandType.StoredProcedure);

            return transaction;
        }


        public List<int> AddTransferTransactions(TransactionDTO transactionSender, TransactionDTO recipient)
        {
            var id = ConString.Query<int>(
                      TransactionStoredProcedure.Transaction_Transfer,
                      param: new
                      {
                          AccountIdSender = transactionSender.AccountId,
                          AccountIdRecipient= recipient.AccountId,
                          Amount= transactionSender.Amount,
                          AmountConverted= recipient.Amount,
                          CurrencySender= transactionSender.Currency,
                          CurrencyRecipient= recipient.Currency,
                          TransactionType= transactionSender.TransactionType
                      },
                      commandType: System.Data.CommandType.StoredProcedure).ToList();

            return id;
        }


        public List<TransactionDTO> GetTransfersByAccountId(int accountId)
        {
            var transfers = ConString.Query<TransactionDTO>(
                      TransactionStoredProcedure.Transaction_GetAllTransfersByAccountId,
                      param: new { accountId },
                      commandType: System.Data.CommandType.StoredProcedure).ToList();

            return transfers;
        }

    }
}
