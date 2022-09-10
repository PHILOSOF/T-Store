using T_Strore.Business.Models;

namespace T_Strore.Business.Producers
{
    public interface ITransactionProducer
    {
        Task NotifyTransaction(TransactionModel model);
    }
}