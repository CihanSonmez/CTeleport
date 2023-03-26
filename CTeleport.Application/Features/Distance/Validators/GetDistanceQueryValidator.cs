using CTeleport.Application.Features.Distance.Queries;
using CTeleport.Localization;
using FluentValidation;

namespace CTeleport.Application.Features.Distance.Validators
{
    public class GetDistanceQueryValidator : AbstractValidator<GetDistanceQuery>
    {
        private readonly Localizer _localizer;

        public GetDistanceQueryValidator(Localizer localizer)
        {
            _localizer = localizer;

            RuleFor(r => r.FirstIataCode)
                .NotNull()
                    .WithMessage(_localizer.FirstIataCodeCannotBeEmpty)
                .Length(3, 3)
                    .WithMessage(_localizer.FirstIataCodeShouldBe3Letter);

            RuleFor(r => r.SecondIataCode)
                .NotNull()
                    .WithMessage(_localizer.SecondIataCodeCannotBeEmpty)
                .Length(3, 3)
                    .WithMessage(_localizer.SecondIataCodeShouldBe3Letter);

        }
    }
}