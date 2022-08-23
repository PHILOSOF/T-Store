using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using T_Store.Models;
using T_Strore.Business.Services;


namespace T_Store.Controllers;

[ApiController]
[Produces("application/json")]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly ITransactionServices _transactionServices;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(ITransactionServices transactionServices, IMapper mapper, ILogger<AccountsController> logger)
    {
        _transactionServices = transactionServices;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id}/balance")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<ActionResult<decimal>> GetBalanceByAccountId([FromRoute] long id)
    {
        _logger.LogInformation("Balance returned");
        return Ok( await _transactionServices.GetBalanceByAccountId(id));
    }

    [HttpGet("{id}/transactions")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetTransactionsByAccountId([FromRoute] long id)
    {
        var transactionsTransfers = await _transactionServices.GetTransactionsByAccountId(id);
        var transactionsModel = _mapper.Map<List<TransactionResponse>>(transactionsTransfers);
        _logger.LogInformation("Transactions by account id returned");
        return Ok(transactionsModel);
    }
}
