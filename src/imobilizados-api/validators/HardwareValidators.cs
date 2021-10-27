using FluentValidation;
using Imobilizados.Application.DTOs;

namespace Imobilizados.API.Validators
{
    public class HardwareValidator : AbstractValidator<Hardware>
    {
        public HardwareValidator()
        {
            RuleFor(h => h.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(h => h.FactoryCode)
                .NotNull()
                .NotEmpty();

            RuleFor(h => h.Description)
                .NotNull()
                .NotEmpty();

            RuleFor(h => h.Brand)
                .NotNull()
                .NotEmpty();

        }
    }
}
