﻿using AutoMapper;
using T_Store.Models.Requests;
using T_Strore.Data;

namespace T_Store;

public class MapperConfigStorage:Profile
{
    public MapperConfigStorage()
    {
        CreateMap<TransactionRequest, TransactionDto>();
        CreateMap<TransactionTransferRequest, TransactionDto>();

    }

}
