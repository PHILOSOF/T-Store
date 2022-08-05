using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using T_Store.Models;
using T_Store.Models.Responses;
using T_Strore.Business.Services;


namespace T_Store.Controllers;

[ApiController]
[Produces("application/json")]
[Route("accounts")]
public class AccountControllers : ControllerBase
{
    private readonly ITransactionServices _transactionServices;
    private readonly IMapper _mapper;

    public AccountControllers(ITransactionServices transactionServices, IMapper mapper)
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
    public async Task<IActionResult> GetTransactionsByAccountId([FromRoute] int id)
    {
        var transactionsTransfers = await _transactionServices.GetTransactionsByAccountId(id);
        var transactionsModel = _mapper.Map<List<TransactionResponse>>(transactionsTransfers);

        return Ok(transactionsModel);

    }
}
