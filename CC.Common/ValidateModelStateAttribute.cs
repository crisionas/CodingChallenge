using CC.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CC.Common
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            switch (context.ModelState.IsValid)
            {
                case true:
                    return;
                case false:
                    {
                        var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                            .SelectMany(v => v.Errors)
                            .Select(v => v.ErrorMessage)
                            .ToList();

                        context.Result = new BadRequestObjectResult(new BaseResponse
                        {
                            ErrorMessage = string.Join(" ", errors)
                        });
                        break;
                    }
            }
        }
    }
}
