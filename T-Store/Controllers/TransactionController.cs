﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using T_Store.Extensions;
using T_Store.Models;
using T_Strore.Business.Services;
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


    [HttpPost("add-deposit")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddDeposit([FromBody] TransactionRequest transaction)
    {
        var id = _transactionServices.AddDeposit(_mapper.Map<TransactionDto>(transaction));
        return Created($"{this.GetRequestPath()}/{id}", id);
    }


    [HttpPost("add-transfer")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> AddTransfer([FromBody] TransactionTransferRequest transferModel)
    {
       
        
     

        
        var recipient = _mapper.Map<List<TransactionDto>>(transferModel);

       // var id = _transactionServices.AddTransfer(recipient, recipient);
        return Created($"{this.GetRequestPath()}/{1}", 1);
    }


    [HttpPost("withdraw-deposit")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> WithdrawDeposit([FromBody] TransactionRequest transaction)
    {
        var id = _transactionServices.WithdrawDeposit(_mapper.Map<TransactionDto>(transaction));
        return Created($"{this.GetRequestPath()}/{id}", id);
    }


    [HttpGet("{accountId}/get-balance")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<int> GetBalanceByAccountId([FromRoute] int accountId)
    {
         
        return Ok(_transactionServices.GetBalanceByAccountId(accountId));
    }


    [HttpGet("{id}/get-transaction")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<TransactionResponse> GetTransactionById([FromRoute] int id)
    {
        var transaction = _transactionServices.GetTransactionById(id);
        return Ok(_mapper.Map<TransactionResponse>(transaction));
    }



    [HttpGet("{accountId}/transactions-with-transfers")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public ActionResult<List<TransactionResponse>> GetTransactionsByAccountId([FromRoute] int accountId)
    {
        var transactionsTransfers = _transactionServices.GetTransactionById(accountId);
        return Ok(_mapper.Map<List<TransactionResponse>>(transactionsTransfers));
    }
}
