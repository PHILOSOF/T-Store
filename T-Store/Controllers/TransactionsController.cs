using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using T_Store.Extensions;
using T_Store.Models;
using T_Strore.Business.Services;
using T_Strore.Data;
namespace T_Store.Controllers;

[ApiController]
[Produces("application/json")]
[Route("transactions")]
public class TransactionsController : Controller
{
    private readonly ITransactionService _transactionServices;
    private readonly IMapper _mapper;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController (ITransactionService transactionServices, IMapper mapper, ILogger<TransactionsController> logger)
    {
        _transactionServices = transactionServices;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpPost("deposit")]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<long>> AddDeposit([FromBody] TransactionRequest transaction)
    {
        _logger.LogInformation("Controller: Request to add deposit");
        var id = await _transactionServices.AddDeposit(_mapper.Map<TransactionDto>(transaction));

        _logger.LogInformation("Controller: Id returned");
        return Created($"{this.GetRequestPath()}/{id}", id);
    }


    [HttpPost("transfer")]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<List<long>>> AddTransfer([FromBody] TransactionTransferRequest transferModel)
    {
         
        var transferModels = _mapper.Map<List<TransactionDto>>(transferModel);

        _logger.LogInformation("Controller: Request to add transfer");
        var id = await _transactionServices.AddTransfer(transferModels);

        _logger.LogInformation("Controller: Transfer ids returned");
        return Created($"{this.GetRequestPath()}/{id}", id);
    }


    [HttpPost("withdraw")]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<long>> Withdraw([FromBody] TransactionRequest transaction)
    {
        _logger.LogInformation("Controller: Request to add withdraw");
        var id = await _transactionServices.Withdraw(_mapper.Map<TransactionDto>(transaction));

        _logger.LogInformation("Controller: Withdraw id returned");
        return Created($"{this.GetRequestPath()}/{id}", id);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public async Task<ActionResult<TransactionResponse>> GetTransactionById([FromRoute] long id)
    {
        _logger.LogInformation("Controller: Request Transactions by id");
        var transaction = await _transactionServices.GetTransactionById(id);

        _logger.LogInformation("Transaction returned");
        return Ok(_mapper.Map<TransactionResponse>(transaction));
    }
}
