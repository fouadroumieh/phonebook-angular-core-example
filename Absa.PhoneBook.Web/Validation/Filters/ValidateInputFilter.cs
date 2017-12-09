using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;

namespace PhoneBook.Web.Validation.Filters
{
    public class ValidateInputFilter : IActionFilter
    {
        private static readonly Dictionary<string, string> ClaimsToAdd = new Dictionary<string, string>
        {
            {"name", "Name"},
            {"role", "Role"}
        };

        private readonly ILogger _logger;

        public ValidateInputFilter(ILogger logger)
        {
            _logger = logger.ForContext<ValidateInputFilter>();
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            using (LogContext.Push(BuildIdentityEnrichers(context.HttpContext.User)))
            {
                _logger.Warning("Model validation failed for {@Input} with validation {@Errors}",
                    context.ActionArguments,
                    context.ModelState?
                        .SelectMany(kvp => kvp.Value.Errors)
                        .Select(e => e.ErrorMessage));
            }
            context.Result = new BadRequestObjectResult(
                from kvp in context.ModelState
                from e in kvp.Value.Errors
                let k = kvp.Key
                select new ValidationError(ValidationError.Type.Input, null, k, e.ErrorMessage));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        protected ILogEventEnricher[] BuildIdentityEnrichers(ClaimsPrincipal user)
        {
            return ClaimsToAdd.Select(kvp =>
                new PropertyEnricher(
                    kvp.Value,
                    user.Claims?.SingleOrDefault(c => c.Type == kvp.Key)?.Value)).ToArray<ILogEventEnricher>();
        }

    }
}
