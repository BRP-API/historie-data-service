using FluentValidation;
using Rvig.HaalCentraalApi.Historie.RequestModels.Historie;

namespace Rvig.HaalCentraalApi.Historie.Validation.RequestModelValidators;

public class RaadpleegMetPeildatumValidator : HaalCentraalHistorieBaseValidator<RaadpleegMetPeildatum>
{
	public RaadpleegMetPeildatumValidator()
	{
		RuleFor(x => x.peildatum)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage(_requiredErrorMessage)
			.Matches(_datePattern).WithMessage(_dateErrorMessage);
	}
}