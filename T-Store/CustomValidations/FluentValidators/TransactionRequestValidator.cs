using FluentValidation;
using T_Store.Infrastructure;
using T_Store.Models;

namespace T_Store.CustomValidations.FluentValidators;

public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
{
    public TransactionRequestValidator()
    {
        RuleFor(t => t.Currency)
            .IsInEnum().WithMessage(ApiErrorMessage.CurrencyRangeError);

    }

}
