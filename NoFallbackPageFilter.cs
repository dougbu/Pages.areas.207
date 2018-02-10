using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pages.areas._207
{
    /// <summary>
    /// An <see cref="IPageFilter"/> implementation that disallows fallbacks from OnPostMeMeMeAsync() to OnPost() and
    /// from missing handler method for the HTTP method to implicit PageResult.
    /// </summary>
    public class NoFallbackPageFilter : IPageFilter
    {
        private const string Handler = "handler";

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            // no-op
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var handlerName = Convert.ToString(context.RouteData.Values[Handler]);
            var httpMethod = context.HttpContext.Request.Method;

            // Work cannot be done in an IResourceFilter because handler has not been selected when one executes.
            // Would perhaps be better to do this in OnPageHandlerSelected(...) and avoid model binding. But.
            // PageHandlerSelectedContext does not have a Result to set.
            if (context.HandlerMethod == null &&
                context.ActionDescriptor.HandlerMethods.Count != 0)
            {
                // Page model contains handlers but this request doesn't match any of them.
                if (string.IsNullOrEmpty(handlerName))
                {
                    context.Result = new NotFoundObjectResult($"Page does not contain handler for '{httpMethod}'.");
                }
                else
                {
                    context.Result = new NotFoundObjectResult(
                        $"Page does not contain handler '{handlerName}' or handler for '{httpMethod}'.");
                }
            }
            else if (context.HandlerMethod != null &&
                string.IsNullOrEmpty(context.HandlerMethod.Name) &&
                !string.IsNullOrEmpty(handlerName))
            {
                // Using a "generic" (unnamed) handler method e.g. OnPostAsync(...) but request asked for a named
                // handler.
                //
                // An IPageHandlerMethodSelector implementation could detect this case and prevent fallback. That
                // would mean we only need to cover the above case here. But, splitting out that part of the logic
                // doesn't seem worthwhile.
                context.Result = new NotFoundObjectResult(
                    $"Page does not contain handler '{handlerName}'. Ignoring handler for '{httpMethod}'.");
            }
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            // no-op
        }
    }
}
