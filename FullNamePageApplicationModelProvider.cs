using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Pages.areas._207
{
    public class FullNamePageApplicationModelProvider : IPageApplicationModelProvider
    {
        // Must execute after DefaultPageApplicationModelProvider. Otherwise, Order isn't too important.
        public int Order => 0;

        public void OnProvidersExecuting(PageApplicationModelProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            for (var i = 0; i < context.PageApplicationModel.HandlerMethods.Count; i++)
            {
                var handler = context.PageApplicationModel.HandlerMethods[i];

                // Leave simply-named handlers like OnPost() alone.
                if (!string.IsNullOrEmpty(handler.HandlerName))
                {
                    // Do *not* trim "On{HTTP method}" from the start or "Async" from the end of the chosen
                    // HandlerName, allowing nameof(method) in asp -page-handler attributes.
                    handler.HandlerName = handler.MethodInfo.Name;
                }
            }
        }

        public void OnProvidersExecuted(PageApplicationModelProviderContext context)
        {
            // no-op
        }
    }
}
