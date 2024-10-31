using FluentValidation;
using Rvig.HaalCentraalApi.Historie.RequestModels.Historie;
using Rvig.HaalCentraalApi.Shared.Validation.RequestModelValidators;

namespace Rvig.HaalCentraalApi.Historie.Validation.RequestModelValidators;

public class HaalCentraalHistorieBaseValidator<T> : HaalCentraalBaseValidator<T> where T : HistorieQuery
{
	public HaalCentraalHistorieBaseValidator()
	{
		RuleFor(x => x.type)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage(_requiredErrorMessage);

		RuleFor(x => x.burgerservicenummer)
			.Cascade(CascadeMode.Stop)
			.Matches(_bsnPattern).WithMessage(GetPatternErrorMessage(_bsnPattern))
			.NotEmpty().WithMessage(_requiredErrorMessage);
	}

	protected const string _bsnPattern = "^[0-9]{9}$";
}