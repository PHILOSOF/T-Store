using FluentValidation;
using T_Store.Infrastructure;
using T_Store.Models;

namespace T_Store.CustomValidations.FluentValidators;

public class TransactionTransferRequestValidator : AbstractValidator<TransactionTransferRequest>
{
    public TransactionTransferRequestValidator()
    {
        RuleFor(t => t.Currency)
            .IsInEnum().WithMessage(ApiErrorMessage.CurrencyRangeError);
        RuleFor(t => t.RecipientCurrency)
           .IsInEnum().WithMessage(ApiErrorMessage.CurrencyRangeError);
    }
}


