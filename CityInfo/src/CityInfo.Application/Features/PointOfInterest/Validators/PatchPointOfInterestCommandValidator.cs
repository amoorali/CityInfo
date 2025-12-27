using CityInfo.Application.Features.PointOfInterest.Commands;
using FluentValidation;

namespace CityInfo.Application.Features.PointOfInterest.Validators
{
    public sealed class PatchPointOfInterestCommandValidator
        : AbstractValidator<PatchPointOfInterestCommand>
    {
        private static readonly HashSet<string> AllowedOps =
            new(StringComparer.OrdinalIgnoreCase)
            { "add", "remove", "replace", "move", "copy", "test" };

        public PatchPointOfInterestCommandValidator()
        {
            RuleFor(x => x.CityId).GreaterThan(0)
                .WithMessage("cityId must be greater than 0.");

            RuleFor(x => x.PointOfInterestId).GreaterThan(0)
                .WithMessage("pointOfInterestId must be greater than 0.");

            RuleFor(x => x.PatchDocument)
                .NotNull().WithMessage("Patch document is required.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.PatchDocument!.Operations)
                        .NotNull().WithMessage("Patch operations are required.")
                        .NotEmpty().WithMessage("Patch document must contain at least one operation.");

                    RuleForEach(x => x.PatchDocument!.Operations)
                        .ChildRules(op =>
                        {
                            op.RuleFor(o => o.op)
                                .Cascade(CascadeMode.Stop)
                                .NotEmpty().WithMessage("Patch operation 'op' is required.")
                                .Must(v => v is not null && AllowedOps.Contains(v))
                                .WithMessage("Patch operation 'op' is invalid.");

                            op.RuleFor(o => o.path)
                                .Cascade(CascadeMode.Stop)
                                .NotEmpty().WithMessage("Patch operation 'path' is required.")
                                .Must(p => p is not null && p.StartsWith("/"))
                                .WithMessage("Patch operation 'path' must start with '/'.");

                            op.RuleFor(o => o.value)
                                .NotNull()
                                .When(o => o.op is not null && (
                                    o.op.Equals("add", StringComparison.OrdinalIgnoreCase) ||
                                    o.op.Equals("replace", StringComparison.OrdinalIgnoreCase) ||
                                    o.op.Equals("test", StringComparison.OrdinalIgnoreCase)
                                    ))
                                .WithMessage("Patch operation 'value' is required for add/replace/test.");
                        });
                });
        }
    }
}
