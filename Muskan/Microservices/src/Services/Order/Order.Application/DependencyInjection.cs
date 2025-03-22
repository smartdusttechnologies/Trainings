using BuildingBlock.Behaviour;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application
{
   public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection service)
        {
  
            service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // MediatR behaviors
            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            service.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            // Ensure FluentValidation validators are registered
            service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //service.AddMediatR(cfg =>
            //{
            //    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //    cfg.AddBehavior(typeof(ValidationBehaviour<,>));
            //    cfg.AddBehavior(typeof(LoggingBehaviour<,>));
            //});
            return service;
        }
    }
}
