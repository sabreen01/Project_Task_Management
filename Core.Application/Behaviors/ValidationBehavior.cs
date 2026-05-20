using Core.Application.Helper.Models;
using FluentValidation;
using MediatR;

namespace Core.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var firstFailure = failures.First();
            var errorMessage = $"{firstFailure.PropertyName}: {firstFailure.ErrorMessage}";
            
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(RequestResult<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var failureMethod = typeof(RequestResult<>)
                    .MakeGenericType(resultType)
                    .GetMethod("Failure", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    
                if (failureMethod != null)
                {
                    return (TResponse)failureMethod.Invoke(null, new object[] { errorMessage })!;
                }
            }

            throw new ValidationException(failures);
        }

        return await next();
    }
}
