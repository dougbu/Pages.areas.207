using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Pages.areas._207
{
    public class RandomModelBinder : IModelBinder
    {
        private Random _random;

        public RandomModelBinder()
        {
            _random = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(string))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var length = _random.Next(minValue: 1, maxValue: 10);
            var builder = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var next = (char)_random.Next(minValue: (int)' ', maxValue: (int)'~');
                builder.Append(next);
            }

            bindingContext.Result = ModelBindingResult.Success(builder.ToString());

            return Task.CompletedTask;
        }
    }
}
