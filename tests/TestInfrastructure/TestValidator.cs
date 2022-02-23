namespace FluentValidation.HttpExtensions.TestInfrastructure
{
    /// <summary>
    /// This validator tests priority of rules applied to an entity.
    /// </summary>
    public class TestValidator : AbstractValidator<TestEntity>
    {
        public TestValidator()
        {
            RuleFor(x => x.IsForbidden)
                .Must(x => x)
                .AsForbidden();

            RuleFor(x => x.IsNotFound)
                .Must(x => x)
                .AsNotFound();

            RuleFor(x => x.IsMethodNotAllowed)
                .Must(x => x)
                .AsMethodNotAllowed();

            RuleFor(x => x.IsNotAcceptable)
                .Must(x => x)
                .AsNotAcceptable();

            RuleFor(x => x.IsConflict)
                .Must(x => x)
                .AsConflict();

            RuleFor(x => x.IsGone)
                .Must(x => x)
                .AsGone();

            RuleFor(x => x.IsLocked)
                .Must(x => x)
                .AsLocked();

            RuleFor(x => x.IsValid)
                .Must(x => x);
       }
    }
}
