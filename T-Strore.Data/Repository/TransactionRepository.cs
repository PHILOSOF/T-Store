﻿using Dapper;


namespace T_Strore.Data
{
    public class TransactionRepository : ServerConnection
    {

        public int AddTransaction(TransactionDTO transaction)
        {
            var id = ConString.QuerySingle<int>(
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
            var balance = ConString.QuerySingle<decimal>(
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
            var transaction = ConString.QuerySingle<TransactionDTO>(
                     TransactionStoredProcedure.Transaction_SelectById,
                     param: new { id },
                     commandType: System.Data.CommandType.StoredProcedure);

            return transaction;
        }
    }
}
