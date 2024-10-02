using FluentValidation;
using Rvig.HaalCentraalApi.Historie.RequestModels.Historie;

namespace Rvig.HaalCentraalApi.Historie.Validation.RequestModelValidators;

public class RaadpleegMetPeriodeValidator : HaalCentraalHistorieBaseValidator<RaadpleegMetPeriode>
{
    public RaadpleegMetPeriodeValidator()
    {
        RuleFor(x => x.datumVan)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(_requiredErrorMessage)
            .Matches(_datePattern).WithMessage(_dateErrorMessage);

        RuleFor(x => x.datumTot)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(_requiredErrorMessage)
            .Matches(_datePattern).WithMessage(_dateErrorMessage);
    }
}
