using Castle.DynamicProxy;
using CoreLibrary.CrossCuttingConcern.Validation;
using CoreLibrary.Utilities.Interceptors;
using FluentValidation;

namespace CoreLibrary.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private readonly Type _validatorType;

        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
                throw new Exception("Bu bir doğrulama sınıfı değildir.");
            
            _validatorType = validatorType;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            IValidator validator = (IValidator)Activator.CreateInstance(_validatorType)!;
            Type entityType = _validatorType.BaseType!.GetGenericArguments()[0];
            IEnumerable<object> entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            
            foreach (object entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}