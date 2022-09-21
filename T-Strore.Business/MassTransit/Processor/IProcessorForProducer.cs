using T_Strore.Business.Models;

namespace T_Strore.Business.Producers
{
    public interface IProcessorForProducer
    {
        Task NotifyTransaction(TransactionModel model);
        Task NotifyTransfer(TransactionModel sender, TransactionModel recipient);
    }
}