﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using T_Store.Extensions;
using T_Store.Models;
using T_Strore.Business.Services;
using T_Strore.Data;

namespace T_Store.Controllers;

[ApiController]
[Produces("application/json")]
[Route("transactions")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionServices _transactionServices;
    private readonly IMapper _mapper;

    public TransactionsController (ITransactionServices transactionServices, IMapper mapper)
    {
        _transactionServices = transactionServices;
        _mapper = mapper;
    }


    [HttpPost("deposit")]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<long>> AddDeposit([FromBody] TransactionRequest transaction)
    {
        var id = await _transactionServices.AddDeposit(_mapper.Map<TransactionDto>(transaction));
        return Created($"{this.GetRequestPath()}/{id}", id);
    }


    [HttpPost("transfer")]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<List<long>>> AddTransfer([FromBody] TransactionTransferRequest transferModel)
    {
         
        var transferModels = _mapper.Map<List<TransactionDto>>(transferModel);
        var id = await _transactionServices.AddTransfer(transferModels);
        return Created($"{this.GetRequestPath()}/{id}", id);
    }


    [HttpPost("withdraw")]
    [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<long>> Withdraw([FromBody] TransactionRequest transaction)
    {
        var id = await _transactionServices.Withdraw(_mapper.Map<TransactionDto>(transaction));
        return Created($"{this.GetRequestPath()}/{id}", id);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<TransactionResponse>> GetTransactionById([FromRoute] long id)
    {
        var transaction = await _transactionServices.GetTransactionById(id);
        return Ok(_mapper.Map<TransactionResponse>(transaction));
    }
}
