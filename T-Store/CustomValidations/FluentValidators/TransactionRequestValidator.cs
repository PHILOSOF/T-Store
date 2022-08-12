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
        RuleFor(t => t.AccountId)
            .GreaterThanOrEqualTo(1).WithMessage(ApiErrorMessage.NumberLessOrEqualZero);
        RuleFor(t => t.Amount)
            .GreaterThanOrEqualTo(1).WithMessage(ApiErrorMessage.NumberLessOrEqualZero);
    }
}
