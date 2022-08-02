using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using T_Store.Models;
using T_Strore.Business.Services;


namespace T_Store.Controllers;

[ApiController]
[Produces("application/json")]
[Route("account")]
public class AccountController : ControllerBase
{
    private readonly ITransactionServices _transactionServices;
    private readonly IMapper _mapper;

    public AccountController(ITransactionServices transactionServices, IMapper mapper)
    {
        _transactionServices = transactionServices;
        _mapper = mapper;
    }


    [HttpGet("{id}/balance")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<decimal>> GetBalanceByAccountId([FromRoute] int id)
    {

        return Ok( await _transactionServices.GetBalanceByAccountId(id));
    }


    [HttpGet("{id}/transactions")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<List<TransactionResponse>>> GetTransactionsByAccountId([FromRoute] int id)
    {
        var transactionsTransfers = await _transactionServices.GetTransactionsByAccountId(id);
        return Ok(_mapper.Map<List<TransactionResponse>>(transactionsTransfers));
    }
}
