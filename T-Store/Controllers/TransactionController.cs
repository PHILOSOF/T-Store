using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using T_Store.Extensions;
using T_Store.Models.Requests;
using T_Strore.Business.Services.Interfaces;
using T_Strore.Data;

namespace T_Store.Controllers;

[ApiController]
[Produces("application/json")]
[Route("transaction")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionServices _transactionServices;
    private readonly IMapper _mapper;

    public TransactionController (ITransactionServices transactionServices, IMapper mapper)
    {
        _transactionServices = transactionServices;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("add-deposit")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddDeposit([FromBody] TransactionRequest transaction)
    {
        var id = _transactionServices.AddDeposit(_mapper.Map<TransactionDto>(transaction));
        return Created($"{this.GetRequestPath()}/{id}", id);
    }

    [HttpPost]
    [Route("withdraw-deposit")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> WithdrawDeposit([FromBody] TransactionRequest transaction)
    {
        var id = _transactionServices.WithdrawDeposit(_mapper.Map<TransactionDto>(transaction));
        return Created($"{this.GetRequestPath()}/{id}", id);
    }

    [HttpPost]
    [Route("add-transfer")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddTransfer([FromBody] TransactionTransferRequest transferModel )
    {
        TransactionDto sender = new()
        {
            AccountId = transferModel.AccountId,
            Amount = transferModel.Amount,
        };

        TransactionDto recipient = new()
        {
           Currency= transferModel.CurrencyRecipient,
           AccountId = transferModel.AccountIdRecipient,
        };


        var id = _transactionServices.AddTransfer(sender, recipient);
        return Created($"{this.GetRequestPath()}/{1}", 1);
    }
}
