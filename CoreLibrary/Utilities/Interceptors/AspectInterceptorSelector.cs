using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using System.Reflection;

namespace CoreLibrary.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            List<MethodInterceptionBaseAttribute> classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();
        
            IEnumerable<MethodInterceptionBaseAttribute> methodAttributes = type.GetMethod(method.Name).GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
        
            classAttributes.AddRange(methodAttributes);

            return (IInterceptor[])classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    } 
}

