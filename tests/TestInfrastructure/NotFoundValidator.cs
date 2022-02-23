namespace FluentValidation.HttpExtensions.TestInfrastructure
{
    /// <summary>
    /// This validator tests what happens if multiple rules with same HTTP error code occur.
    /// </summary>
    public class NotFoundValidator : AbstractValidator<NotFoundEntity>
    {
        public NotFoundValidator()
        {
            RuleFor(x => x.Name)
                .Must(x => x == "Existing")
                .AsNotFound()
                .WithMessage("Could not find {PropertyName} {PropertyValue}");

            RuleFor(x => x.Address)
                .Must(x => x == "Existing")
                .AsNotFound()
                .WithMessage("Could not find {PropertyName} {PropertyValue}");
        }
    }
}
