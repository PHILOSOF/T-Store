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
    private readonly ITransactionService _transactionServices;
    private readonly IMapper _mapper;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(ITransactionService transactionServices, IMapper mapper, ILogger<AccountsController> logger)
    {
        _transactionServices = transactionServices;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{id}/balance")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    public async Task<ActionResult<decimal>> GetBalanceByAccountId([FromRoute] long id)
    {
        _logger.LogInformation("Controller: Request a balance");
        return Ok( await _transactionServices.GetBalanceByAccountId(id));
    }

    [HttpGet("{id}/transactions")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactionsByAccountId([FromRoute] long id)
    {
        _logger.LogInformation("Controller: Request a transactions by account id");
        var transactions = await _transactionServices.GetTransactionsByAccountId(id);
        var transactionsModel = _mapper.Map<List<TransactionResponse>>(transactions);
    
        return Ok(transactionsModel);
    }
}
 