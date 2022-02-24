# Pankraty.FluentValidation.HttpExtensions

[FluentValidation](https://github.com/FluentValidation/FluentValidation) is a powerful library that seamlessly integrates
with ASP.NET and gives us the ability to keep all validation logic well-organized. However, if we want to implement comprehensive
Web API utilizing different HTTP codes, such as `NotFound` (404), `Forbidden` (403), `Conflict` (409), and so on, we have to
keep these pieces of logic apart from other validation rules because built-in validation will only return `BadRequest` response 
in case of any failure.

With this library, we can extend the standard validation rules of `FluentValidation` by defining which HTTP statuses should be
produced when a certain rule is violated:

```
RuleFor(x => x.OrderId)
    .Must(BeExistingOrder)
    .AsNotFound() // <-- The addition to produce error 404 instead of BadRequest  
    .WithMessage("Order does not exist.");
```

Currently, the library supports these HTTP statuses:
* 403 - Forbidden,
* 404 - NotFound,
* 405 - MethodNotAllowed,
* 406 - NotAcceptable,
* 409 - Conflict,
* 410 - Gone,
* 423 - Locked
              
If you need more statuses, contributions are welcome.

When there are multiple failing rules, they are ranged by priority, where `Forbidden` has the highest priority and `BadRequest` 
(default) - the lowest. All errors with the top priority are combined and returned as a single `ValidationProblemDetails`
object inside a response with the corresponding HTTP status; the rest of the errors are removed from the response.

For example, if there are following rules defined for an object:
```
RuleFor(x => x.OrderId).Must(BeExistingOrder).AsNotFound().WithMessage("Order does not exist.");
RuleFor(x => x.CustomerId).Must(BeExistingCustomer).AsNotFound().WithMessage("Customer does not exist.");
RuleFor(x => x.OrderId).Must(BelongToCurrentAccount).AsForbidden().WithMessage("You do not have permissions to edit the order.");
RuleFor(x => x.Quantity).GreaterThan(0);
```
and all of them failed, then the user will receive a response with the status 403 (Forbidden) and a single error message
"You do not have permissions to edit the order.". But if this rule passed, then the user will get error 404 with two 
messages:
```
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.3",
    "title": "One or more validation errors occurred.",
    "status": 404,
    "errors": {
        "": [
            "Order does not exist.",
            "Customer does not exist."            
        ]
    }
}
```
   
The error for the `Quantity` field will be shown only when there are no errors with custom HTTP statuses.

## Usage

1. Install NuGet package `Pankraty.FluentValidation.HttpExtensions` using command-line or GUI of your choice.
2. In `Startup.cs`, after registering `FluentValidation`, register HTTP extensions to it:
    ```
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddFluentValidation()
            .AddFluentValidationHttpExtensions();
    }
    ```
3. In validators, use extension methods from `FluentValidation.HttpExtensions` namespace on the rules that must produce
custom HTTP responses: `AsForbidden()`, `AsNotFound()`, etc.
   
Notice that internally these methods override property names for the rules they extend, therefore, you should not be using 
`OverridePropertyName` on such rules. However, this does not affect `{PropertyName}` placeholder that can safely be used 
in error message templates, like this:
```
RuleFor(x => x.OrderId)
    .Must(BeExistingOrder)
    .AsNotFound()
    .WithMessage("'{PropertyName}' must point to an existing order.");
```
