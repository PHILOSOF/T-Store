using FluentValidation;
using T_Store.Infrastructure;
using T_Store.Models;
using IncredibleBackendContracts.Enums;

namespace T_Store.CustomValidations.FluentValidators;

public class TransactionTransferRequestValidator : AbstractValidator<TransactionTransferRequest>
{
    public TransactionTransferRequestValidator()
    {
        RuleFor(t => t.RecipientCurrency)
           .IsInEnum().WithMessage(ApiErrorMessage.CurrencyRangeError);
        RuleFor(t => t.RecipientAccountId)
            .GreaterThanOrEqualTo(1).WithMessage(ApiErrorMessage.NumberLessOrEqualZero);
        RuleFor(t => t.Currency)
           .IsInEnum().WithMessage(ApiErrorMessage.CurrencyRangeError)
           .NotEqual(t => t.RecipientCurrency).WithMessage(ApiErrorMessage.SameCurrency);
        RuleFor(t => t.AccountId)
            .GreaterThanOrEqualTo(1).WithMessage(ApiErrorMessage.NumberLessOrEqualZero)
            .NotEqual(t=>t.RecipientAccountId).WithMessage(ApiErrorMessage.SameAccountId);
        RuleFor(t => t.Amount)
            .GreaterThanOrEqualTo(1).WithMessage(ApiErrorMessage.NumberLessOrEqualZero);
    }
}


