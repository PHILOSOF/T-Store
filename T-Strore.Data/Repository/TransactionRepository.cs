using Dapper;
using System.Data.SqlClient;

namespace T_Strore.Data
{
    public class TransactionRepository
    {
        public string connectionString = @"Server=.;Database=T-Store.DB;Trusted_Connection=True;";

        public int AddTransaction(TransactionDTO transaction)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

               var id = connection.QuerySingle<int>(
                     TransactionStoredProcedure.Insert_Transactiont,
                     param: new
                     {
                         transaction.AccountId,
                         transaction.Type,
                         transaction.Amount,    
                         transaction.Currency
                        
                     },
                     commandType: System.Data.CommandType.StoredProcedure);
                return id;
            }
        }
        public decimal GetBalanceByAccountId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var balance = connection.QuerySingle<decimal>(
                      TransactionStoredProcedure.Select_BalanceByAccountId,
                      param: new
                      {                   
                          id
                      },
                      commandType: System.Data.CommandType.StoredProcedure);
                return balance;
            }
        }

        public List<TransactionDTO> GetTransactionsByAccountId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var Transactions = connection.Query<TransactionDTO>(
                      TransactionStoredProcedure.Select_BalanceByAccountId,
                      param: new
                      {
                          id
                      },
                      commandType: System.Data.CommandType.StoredProcedure).ToList();
                return Transactions;
            }
        }

        public TransactionDTO GetTransactionById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var transaction = connection.QuerySingle<TransactionDTO>(
                      TransactionStoredProcedure.Select_BalanceByAccountId,
                      param: new
                      {
                          id
                      },
                      commandType: System.Data.CommandType.StoredProcedure);
                return transaction;
            }
        }
    }

}
