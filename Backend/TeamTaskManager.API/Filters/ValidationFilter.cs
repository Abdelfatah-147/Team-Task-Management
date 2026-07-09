using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TeamTaskManager.API.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Where(x => x.Value!.Errors.Any())
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value!.Errors.Select(e => e.ErrorMessage)
                    });

                context.Result = new BadRequestObjectResult(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });
                return;
            }
            await next();
        }
    }
}
