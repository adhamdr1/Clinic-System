namespace Clinic_System.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    var messages = failures.Select(x => x.PropertyName + ": " + x.ErrorMessage).ToList();

                    throw new ApiException(
                        "Validation Failed",
                        400,
                        messages
                    );
                }
            }
            return await next();
        }
    }
}

/*
 {
  "fullName": "string",
  "gender": "Male",
  "dateOfBirth": "2000-02-09",
  "phone": "123",
  "address": "Mansora",
  "specialization": "string",
  "userName": "1doma",
  "email": "adham#g.c",
  "password": "doma.drr",
  "confirmPassword": "doma.drd"
}
 */