using FluentValidation;
using FluentValidation.Results;

namespace CoreLibrary.CrossCuttingConcern.Validation
{
    public class ValidationTool
    {
        public static void Validate(IValidator validator, object entity)
        {
            ValidationContext<object> context = new(entity);
            ValidationResult result = validator.Validate(context);
            
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
                // return new DataResult { ErrorMessageList = result.Errors.Select(c => c.ErrorMessage).ToList() };
            }
        }
    }
}