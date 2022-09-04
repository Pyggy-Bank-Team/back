using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PiggyBank.WebApi.Responses;

namespace PiggyBank.WebApi.Filters
{
    public class ValidateRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.SelectMany(keyValuePair => keyValuePair.Value.Errors).Select(modelError => modelError.ErrorMessage).ToArray();
                var errorResponse = new ErrorResponse("InvalidRequest", errors);
                context.Result = new BadRequestObjectResult(errorResponse);
            }
        }
    }
}