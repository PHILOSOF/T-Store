﻿using AutoMapper;
using T_Store.Models;
using T_Strore.Data;

namespace T_Store.Mapper
{
    public class MapperHelper :ITypeConverter<TransactionTransferRequest, List<TransactionDto>>
    {
        public  List<TransactionDto> Convert(TransactionTransferRequest source, List<TransactionDto> destination, ResolutionContext context)
        {

            destination = new List<TransactionDto>()
             {
                new TransactionDto()
                {
                    AccountId = source.AccountId,
                    Amount = source.Amount,
                    Currency = source.Currency

                },
                new TransactionDto()
                {
                    AccountId = source.AccountIdRecipient,
                    Currency = source.CurrencyRecipient

                },
             };

            return context.Mapper.Map<List<TransactionDto>>(destination);
        }
    }
}
