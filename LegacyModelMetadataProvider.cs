using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Options;

namespace Pages.areas._207
{
    public class LegacyModelMetadataProvider : IModelMetadataProvider
    {
        private readonly IModelMetadataProvider _innerProvider;

        public LegacyModelMetadataProvider(
            ICompositeMetadataDetailsProvider detailsProvider,
            IOptions<MvcOptions> optionsAccessor)
        {
            _innerProvider = new DefaultModelMetadataProvider(detailsProvider, optionsAccessor);
        }

        public IEnumerable<ModelMetadata> GetMetadataForProperties(Type modelType)
        {
            return _innerProvider.GetMetadataForProperties(modelType);
        }

        public ModelMetadata GetMetadataForType(Type modelType)
        {
            return _innerProvider.GetMetadataForType(modelType);
        }
    }
}
