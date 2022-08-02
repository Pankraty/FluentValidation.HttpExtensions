using System.Net;
using FluentValidation.HttpExtensions.Internal;

namespace FluentValidation.HttpExtensions
{
    public static class IRuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> AsForbidden<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder) =>
            ruleBuilder.UseHttpCode(HttpStatusCode.Forbidden);

        public static IRuleBuilderOptions<T, TProperty> AsNotFound<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder) =>
            ruleBuilder.UseHttpCode(HttpStatusCode.NotFound);

        public static IRuleBuilderOptions<T, TProperty> AsMethodNotAllowed<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder) =>
            ruleBuilder.UseHttpCode(HttpStatusCode.MethodNotAllowed);

        public static IRuleBuilderOptions<T, TProperty> AsNotAcceptable<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder) =>
            ruleBuilder.UseHttpCode(HttpStatusCode.NotAcceptable);

        public static IRuleBuilderOptions<T, TProperty> AsConflict<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder) =>
            ruleBuilder.UseHttpCode(HttpStatusCode.Conflict);

        public static IRuleBuilderOptions<T, TProperty> AsGone<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder) =>
            ruleBuilder.UseHttpCode(HttpStatusCode.Gone);

        public static IRuleBuilderOptions<T, TProperty> AsLocked<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder) =>
            ruleBuilder.UseHttpCode(HttpStatusCode.Locked);

        private static string BuildPropertyName(HttpStatusCode httpStatusCode) =>
            $"{ErrorStatusConst.Prefix}{(int)httpStatusCode}";

        private static IRuleBuilderOptions<T, TProperty> UseHttpCode<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> ruleBuilder, HttpStatusCode httpStatusCode)
        {
            string propertyName = "";

            var options = ruleBuilder.Configure(x => propertyName = x.GetDisplayName(null));

            if (!string.IsNullOrEmpty(propertyName))
            {
                options.WithName(propertyName);
            }

            options.OverridePropertyName(BuildPropertyName(httpStatusCode));

            return options;
        }
    }
}
